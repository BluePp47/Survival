using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter(Collider other)
    {
        IInventoryHolder inventoryHolder = other.GetComponent<IInventoryHolder>();
        if (inventoryHolder != null)
        {
            inventoryHolder.GetInventory().AddItem(item);
            Destroy(gameObject);
        }
    }
}