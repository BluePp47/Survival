using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInventoryHolder
{
    Inventory GetInventory();
}

public class Item : MonoBehaviour
{
    public Resource resourceComponent;
    public Equipment equipmentComponent;

    void Awake()
    {
        resourceComponent = GetComponent<Resource>();
        equipmentComponent = GetComponent<Equipment>();
    }

    public bool IsResource()
    {
        return resourceComponent != null;
    }

    public bool IsEquipable()
    {
        return equipmentComponent != null;
    }
}