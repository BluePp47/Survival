using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceManager : MonoBehaviour
{
    // public static ResorceManager Instance;

    // public int wood;
    // public int stone;

    // private void Awake() => Instance = this;

    // public bool HasEnough(int woodCost, int stoneCost) =>
    //     wood >= woodCost && stone >= stoneCost;

    // public void Spend(int woodCost, int stoneCost)
    // {
    //     wood -= woodCost;
    //     stone -= stoneCost;
    // }
    
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
