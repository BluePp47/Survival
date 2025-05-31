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


    public int GetAmount(ItemData item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

}
