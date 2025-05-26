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
                // ✅ 플레이어가 멈춰도 계속 추적
                ChasePlayer();

                if (distanceToPlayer <= EnemyStats.attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                else if (distanceToPlayer >= EnemyStats.returnDistance)
                {
                    currentState = EnemyState.Return;
                }
                break;

            case EnemyState.Attack:
                StopMovement();
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
}
