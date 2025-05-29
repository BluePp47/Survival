using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int quantity;

    public InventoryItem(ItemData itemData, int amount = 1)
    {
        data = itemData;
        quantity = amount;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public PlayerController player;
    public UIInventory uiInventory;

    public void AddItem(ItemData item)
    {
        Debug.Log("아이템 추가됨: " + item.name);

        if (item.canStack)
        {
            InventoryItem slot = items.Find(i => i.data == item);
            if (slot != null && slot.quantity < item.maxStackAmount)
            {
                slot.quantity++;
                uiInventory.RefreshUI(items);
                return;
            }
        }

        items.Add(new InventoryItem(item));

        player.itemData = item;
        player.addItem?.Invoke();

        Debug.Log("uiInventory is null? " + (uiInventory == null));

        uiInventory.RefreshUI(items);
    }

    public void UseItem(ItemData item)
    {
        InventoryItem invItem = items.Find(i => i.data == item);
        if (invItem == null) return;

        if (item.type == ItemType.Consumable)
        {
            foreach (var effect in item.consumables)
            {
                switch (effect.type)
                {
                    case ConsumableType.Health:
                        player.condition.Add(effect.value);
                        break;
                    case ConsumableType.Hunger:
                        player.condition.Add(effect.value);
                        break;
                    case ConsumableType.Thirsty:
                        player.condition.Add(effect.value);
                        break;
                }
            }
        }

        invItem.quantity--;
        if (invItem.quantity <= 0)
            items.Remove(invItem);
    }


    public void RemoveItem(ItemData item, int count = 1)
    {
        InventoryItem invItem = items.Find(i => i.data == item);
        if (invItem == null)
        {
            Debug.LogWarning("[Inventory] 제거 대상 아이템 없음");
            return;
        }

        invItem.quantity -= count;
        Debug.Log($"[Inventory] {item.displayName} 수량 감소됨 → 남은 수량: {invItem.quantity}");

        if (invItem.quantity <= 0)
        {
            items.Remove(invItem);
            Debug.Log("[Inventory] 수량 0 → 아이템 제거");
        }

        uiInventory.RefreshUI(items);
    }

}