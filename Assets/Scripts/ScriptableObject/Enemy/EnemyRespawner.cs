using UnityEngine;
using System.Collections;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // 재생성할 원본 프리팹
    public float respawnDelay = 10f;

    public void StartRespawn(Vector3 position)
    {
        StartCoroutine(RespawnCoroutine(position));
    }

    private IEnumerator RespawnCoroutine(Vector3 position)
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        Debug.Log($"[Respawner] 적이 재생성됨: {newEnemy.name}");
    }
}
