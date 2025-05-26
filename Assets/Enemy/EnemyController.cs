using UnityEngine;
using System.Collections;

public class EnemyController : BaseCharacterController
{
    public Transform player;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    private EnemyStats EnemyStats => (EnemyStats)stats;

    private Vector3 spawnPoint;
    private Vector3 wanderTarget;
    private float wanderTimer;

    private float attackCooldown = 0f;


    private enum EnemyState { Wander, Chase, Attack, Return }
    private EnemyState currentState = EnemyState.Wander;



    protected override void Awake()
    {
        base.Awake();
        spawnPoint = transform.position;
        SetNewWanderTarget();
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        

        switch (currentState)
        {
            case EnemyState.Wander:
                Wander();
                if (distanceToPlayer <= EnemyStats.detectionRange)
                {
                    SwitchToChase();
                }
                break;

            case EnemyState.Chase:
                ChasePlayer();
                if (distanceToPlayer <= EnemyStats.attackRange)
                {
                    currentState = EnemyState.Attack;
                    attackCooldown = 0f;
                   
                }
                else if (distanceToPlayer >= EnemyStats.returnDistance)
                    currentState = EnemyState.Return;
                break;

            case EnemyState.Attack:
                StopMovement();

                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0f)
                {
                   
                    AttackPlayer();
                    attackCooldown = EnemyStats.attackInterval;
                }

                if (distanceToPlayer > EnemyStats.attackRange)
                {
                    currentState = EnemyState.Chase;
                }
                break;


            case EnemyState.Return:
                ReturnToSpawn();
                if (Vector3.Distance(transform.position, spawnPoint) < 1f)
                {
                    currentState = EnemyState.Wander;
                    SetNewWanderTarget();
                }
                break;
        }
    }

    void SwitchToChase()
    {
        currentState = EnemyState.Chase;
        Debug.Log("[Enemy] 플레이어 감지 → 추적 시작");
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        Vector3 direction = (wanderTarget - transform.position).normalized;

        velocity.x = direction.x * EnemyStats.moveSpeed;
        velocity.z = direction.z * EnemyStats.moveSpeed;

        if (wanderTimer <= 0 || Vector3.Distance(transform.position, wanderTarget) < 1f)
        {
            SetNewWanderTarget();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        velocity.x = direction.x * EnemyStats.moveSpeed * 1.2f;
        velocity.z = direction.z * EnemyStats.moveSpeed * 1.2f;

        // 🔄 자연스럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }

    void ReturnToSpawn()
    {
        Vector3 direction = (spawnPoint - transform.position).normalized;

        velocity.x = direction.x * EnemyStats.moveSpeed;
        velocity.z = direction.z * EnemyStats.moveSpeed;
    }

    void StopMovement()
    {
        velocity.x = 0f;
        velocity.z = 0f;
    }

    void SetNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = spawnPoint + new Vector3(randomCircle.x, 0f, randomCircle.y);
        wanderTimer = wanderInterval;
    }

    void AttackPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("[Enemy] player가 비어 있음!");
            return;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Debug.Log($"[Enemy] 플레이어에게 공격! {EnemyStats.attackPower} 데미지");
            playerController.TakeDamage(EnemyStats.attackPower);
        }
        else
        {
            Debug.LogWarning($"⚠ player 오브젝트에는 PlayerController가 없음 → {player.name}");
        }
    }


    protected override void Die()
    {
        base.Die();

        // 1. 오브젝트 비활성화
        gameObject.SetActive(false);

        // 2. 드랍 아이템 처리
        EnemyStats enemyStats = stats as EnemyStats;
        if (enemyStats != null && enemyStats.dropItem != null)
        {
            TryDropItem(enemyStats.dropItem);
        }
    }

    void TryDropItem(DropItemData dropData)
    {
        int roll = Random.Range(0, 100);
        if (roll < dropData.dropChance && dropData.itemPrefab != null)
        {
            Instantiate(dropData.itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            Debug.Log($"드랍 아이템 생성됨: {dropData.itemPrefab.name}");
        }
        else
        {
            Debug.Log("아이템 드랍 실패 (확률 미충족)");
        }
    }

}
