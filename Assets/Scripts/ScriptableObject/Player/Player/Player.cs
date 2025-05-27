using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IInventoryHolder
{
    public Inventory inventory;

    public Inventory GetInventory()
    {
        return inventory;
    }
}
