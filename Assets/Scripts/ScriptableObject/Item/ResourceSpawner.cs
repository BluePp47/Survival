using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject[] resourcePrefabs;
    public int spawnCount = 10;
    public Vector2 spawnAreaSize = new Vector2(50f, 50f);
    public Vector3 centerPosition = Vector3.zero;
    public LayerMask groundLayer;

    private List<Vector3> occupiedPositions = new List<Vector3>();
    private float minDistance = 2f;

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnOne();
        }
    }

    public void SpawnOne()
    {
        for (int attempts = 0; attempts < 30; attempts++)
        {
            float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float z = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
            Vector3 rayOrigin = new Vector3(centerPosition.x + x, centerPosition.y + 50f, centerPosition.z + z);

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 spawnPos = hit.point;

                bool tooClose = false;
                foreach (var pos in occupiedPositions)
                {
                    if (Vector3.Distance(pos, spawnPos) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose) continue;

                GameObject prefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];

                GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
                occupiedPositions.Add(spawnPos);

                Resource res = obj.GetComponent<Resource>();
                if (res != null)
                    res.spawner = this;

                break;
            }
        }
    }

    public void UnregisterPosition(Vector3 pos)
    {
        occupiedPositions.Remove(pos);
    }
}
