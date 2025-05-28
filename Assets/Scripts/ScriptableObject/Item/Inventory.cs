using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
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
    public Player player;

    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            InventoryItem slot = items.Find(i => i.data == item);
            if (slot != null && slot.quantity < item.maxStackAmount)
            {
                slot.quantity++;
                return;
            }
        }

        items.Add(new InventoryItem(item));
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
                        player.RestoreHealth(effect.value);
                        break;
                    case ConsumableType.Hunger:
                        player.RestoreHunger(effect.value);
                        break;
                    case ConsumableType.Thirsty:
                        player.RestoreThirst(effect.value);
                        break;
                }
            }
        }

        invItem.quantity--;

        if (invItem.quantity <= 0)
        {
            items.Remove(invItem);
        }
    }
}
