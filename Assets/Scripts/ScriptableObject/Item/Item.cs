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
    public Consumption consumptionComponent;

    void Awake()
    {
        resourceComponent = GetComponent<Resource>();
        equipmentComponent = GetComponent<Equipment>();
        consumptionComponent = GetComponent<Consumption>();
    }

    public bool IsResource()
    {
        return resourceComponent != null;
    }

    public bool IsEquipable()
    {
        return equipmentComponent != null;
    }

    public bool IsConsumable()
    {
        return consumptionComponent != null;
    }
}