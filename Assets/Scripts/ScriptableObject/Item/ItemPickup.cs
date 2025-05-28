using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter(Collider other)
    {
        var holder = other.GetComponentInParent<IInventoryHolder>();
        if (holder == null) return;

        Inventory inv = holder.GetInventory();
        if (inv == null || item == null) return;

        inv.AddItem(item);
        Destroy(gameObject);
    }
}