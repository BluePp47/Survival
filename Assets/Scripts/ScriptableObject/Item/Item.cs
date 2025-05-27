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
<<<<<<< HEAD
    public Consumption consumptionComponent;
=======
>>>>>>> 22004982856045d28e6b925938831d48acf7b099

    void Awake()
    {
        resourceComponent = GetComponent<Resource>();
        equipmentComponent = GetComponent<Equipment>();
<<<<<<< HEAD
        consumptionComponent = GetComponent<Consumption>();
=======
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    }

    public bool IsResource()
    {
        return resourceComponent != null;
    }

    public bool IsEquipable()
    {
        return equipmentComponent != null;
    }
<<<<<<< HEAD

    public bool IsConsumable()
    {
        return consumptionComponent != null;
    }
=======
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
}