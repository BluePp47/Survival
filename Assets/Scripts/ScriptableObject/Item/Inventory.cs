using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int quantity;
    public bool isEquipped;

    public InventoryItem(ItemData itemData, int amount = 1)
    {
        data = itemData;
        quantity = amount;
        isEquipped = false;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public PlayerController player;
    public UIInventory uiInventory;

    public event Action OnInventoryChanged;

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

        OnInventoryChanged?.Invoke();
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
                        player.healthCondition?.Add(effect.value);
                        break;
                    case ConsumableType.Hunger:
                        player.hungerCondition?.Add(effect.value);
                        break;
                    case ConsumableType.Thirsty:
                        player.thirstCondition?.Add(effect.value);
                        break;
                }
            }
        }

        invItem.quantity--;
        if (invItem.quantity <= 0)
            items.Remove(invItem);

        uiInventory.RefreshUI(items);
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

     public bool HasEnoughItem(ItemData item, int requiredAmount)
    {
        InventoryItem invItem = items.Find(i => i.data == item);
        return invItem != null && invItem.quantity >= requiredAmount;
    }
    public void SpendItem(ItemData item, int amount)
    {
        InventoryItem invItem = items.Find(i => i.data == item);
        if (invItem != null)
        {
            invItem.quantity -= amount;
            if (invItem.quantity <= 0)
            {
                items.Remove(invItem);
            }

            uiInventory.RefreshUI(items);
        }
    }
     
     // 인벤토리 내역 저장
    public SaveData GetSaveData()
    {
        SaveData data = new SaveData();

        foreach (var item in items)
        {
            data.inventoryItems.Add(new InventoryItemSaveData
            {
                itemID = item.data.displayName,
                quantity = item.quantity,
                isEquipped = item.isEquipped
            });
        }

        return data;
    }

    // 인벤토리 내역 불러오기
    public void LoadInventory(List<InventoryItemSaveData> savedItems)
    {
            if (uiInventory == null)
    {
        uiInventory = FindObjectOfType<UIInventory>();
        if (uiInventory == null)
            Debug.LogError("❌ UIInventory를 씬에서 찾지 못했습니다.");
    }
        Debug.Log("📦 LoadInventory 호출됨");
        items.Clear();

        foreach (var saved in savedItems)
        {
            ItemData itemData = ItemDatabaseManager.Instance.GetItemById(saved.itemID);
            if (itemData != null)
            {
                items.Add(new InventoryItem(itemData, saved.quantity)
                {
                    isEquipped = saved.isEquipped
                });
            }
        }
        

        uiInventory.RefreshUI(items);
    }
}