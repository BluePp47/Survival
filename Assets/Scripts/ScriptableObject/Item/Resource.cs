using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity = 5;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (capacity <= 0) return;

        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity--;

            Vector3 dropPos = hitPoint + Vector3.up * 0.5f + Random.insideUnitSphere * 0.3f;
            dropPos.y = hitPoint.y + 0.5f;

            GameObject drop = Instantiate(itemToGive.dropPrefab, dropPos, Quaternion.identity);

            ItemPickup pickup = drop.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.item = itemToGive;
            }
            else
            {
                Debug.LogWarning("드롭 프리팹 없습니다");
            }
        }

        if (capacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
