using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceManager : MonoBehaviour
{
    public static ResorceManager Instance;

    private Dictionary<ItemData, int> inventory = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

  
    public void Add(ItemData item, int amount)
    {
        if (!inventory.ContainsKey(item))
            inventory[item] = 0;
        inventory[item] += amount;
    }


    public int GetAmount(ItemData targetItem)
    {   
        Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        List<InventoryItem> inventoryItems = inventory.items;
        foreach (var item in inventoryItems)
        {
        if (item.data.displayName == targetItem.displayName) // 또는 item.data.id == targetItem.id
        {
            return item.quantity;
        }
        }

    return 0;
    }

}
