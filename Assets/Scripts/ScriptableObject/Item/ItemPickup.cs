using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter(Collider other)
    {
        IInventoryHolder inventoryHolder = other.GetComponent<IInventoryHolder>();
        if (inventoryHolder != null && item != null)
        {
            inventoryHolder.GetInventory().AddItem(item);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning($"[ItemPickup] 아이템 추가 실패 - inventoryHolder 또는 item이 null");
        }
    }
}