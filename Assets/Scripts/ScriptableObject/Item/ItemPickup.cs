using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public float pickupDelay = 0.5f;
    private bool isPickingUp = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isPickingUp)
        {
            StartCoroutine(DelayedPickup(other));
        }
    }

    private IEnumerator DelayedPickup(Collider other)
    {
        isPickingUp = true;
        yield return new WaitForSeconds(pickupDelay);

        // 이 오브젝트나 컴포넌트가 파괴되었는지 확인
        if (this == null || gameObject == null) yield break;

        var holder = other != null ? other.GetComponentInParent<IInventoryHolder>() : null;
        if (holder == null) { isPickingUp = false; yield break; }

        Inventory inv = holder.GetInventory();
        if (inv == null || item == null) { isPickingUp = false; yield break; }

        inv.AddItem(item);

        // 아이템 오브젝트가 여전히 존재하면 파괴
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
