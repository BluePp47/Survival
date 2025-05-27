using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
<<<<<<< HEAD
    public int capacity = 5;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (capacity <= 0) return;

=======
    public int capacity;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

<<<<<<< HEAD
            capacity--;

            Vector3 dropPos = hitPoint + Vector3.up * 0.5f + Random.insideUnitSphere * 0.3f;
            dropPos.y = hitPoint.y + 0.5f;

            Instantiate(itemToGive.dropPrefab, dropPos, Quaternion.identity);
=======
            capacity -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
        }

        if (capacity <= 0)
        {
            Destroy(gameObject);
        }
<<<<<<< HEAD
=======

        var player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.inventory.AddItem(itemToGive);
        }
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    }
}
