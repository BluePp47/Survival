using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter(Collider other)
    {
        var inventoryHolder = other.GetComponentInParent<IInventoryHolder>();

        if (inventoryHolder != null && item != null)
        {
            inventoryHolder.GetInventory().AddItem(item);

            // UI 갱신을 위해 이벤트 호출
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.itemData = item;  // AddItem()용 데이터 설정
                player.onAddItem?.Invoke();  // UIInventory에서 등록한 이벤트 호출
            }

            Destroy(gameObject);
        }
    }
}