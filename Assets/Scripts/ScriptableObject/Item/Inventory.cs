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
            return;
        }

        invItem.quantity -= count;

        if (invItem.quantity <= 0)
        {
            items.Remove(invItem);
        }

        uiInventory.RefreshUI(items);
    }

}