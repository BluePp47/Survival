using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌한 오브젝트: " + other.name);

        Debug.Log("ItemPickup 충돌 대상: " + other.name);

        var inventoryHolder = other.GetComponentInParent<IInventoryHolder>();
        if (inventoryHolder == null)
        {
            Debug.LogWarning($"[ItemPickup] IInventoryHolder를 찾을 수 없습니다. {other.name}의 부모 트리를 확인하세요.");
            return;
        }

        if (item == null)
        {
            Debug.LogWarning("ItemPickup.item이 설정되지 않았습니다!");
            return;
        }

        var inventory = inventoryHolder.GetInventory();
        if (inventory == null)
        {
            Debug.LogWarning("Inventory가 null입니다.");
            return;
        }

        inventory.AddItem(item);

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.itemData = item;
            player.onAddItem?.Invoke();
        }

        Destroy(gameObject);
    }

}