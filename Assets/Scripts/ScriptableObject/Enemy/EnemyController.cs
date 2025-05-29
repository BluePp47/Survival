using UnityEngine;
using System.Collections;

public class EnemyController : BaseCharacterController
{
    private Transform player;

    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    private EnemyStats EnemyStats => (EnemyStats)stats;

    private Vector3 spawnPoint;
    private Vector3 wanderTarget;
    private float wanderTimer;

    private float attackCooldown = 0f;

    private Coroutine respawnCoroutine;

    private bool isResting = false;
    private float restTimer = 0f;




    private enum EnemyState { Wander, Chase, Attack, Return }
    private EnemyState currentState = EnemyState.Wander;



    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        SetNewWanderTarget();

        // ✅ 자동으로 플레이어 감지
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
            Debug.Log("[Enemy] 플레이어 자동 감지 완료");
        }
        else
        {
            Debug.LogWarning("[Enemy] 'Player' 태그를 가진 오브젝트가 없습니다!");
        }
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
        if (isResting)
        {
            restTimer -= Time.deltaTime;
            velocity.x = 0f;
            velocity.z = 0f;
            UpdateRunAnimation();

            if (restTimer <= 0f)
            {
                Debug.Log("[Enemy] 휴식 종료, 배회 재개");
                SetNewWanderTarget(); // 휴식 끝 → 새로운 목표
            }
            return;
        }

        wanderTimer -= Time.deltaTime;

        Vector3 direction = (wanderTarget - transform.position);
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
        }

        Vector3 moveDir = direction.normalized;
        velocity.x = moveDir.x * EnemyStats.moveSpeed;
        velocity.z = moveDir.z * EnemyStats.moveSpeed;

        if (wanderTimer <= 0 || Vector3.Distance(transform.position, wanderTarget) < 1f)
        {
            SetNewWanderTarget();
        }

        UpdateRunAnimation();
    }


    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        velocity.x = direction.x * EnemyStats.moveSpeed * 1.2f;
        velocity.z = direction.z * EnemyStats.moveSpeed * 1.2f;

        // 🔄 자연스럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        UpdateRunAnimation();

    }

    void ReturnToSpawn()
    {
        Vector3 direction = (spawnPoint - transform.position).normalized;

        velocity.x = direction.x * EnemyStats.moveSpeed;
        velocity.z = direction.z * EnemyStats.moveSpeed;
        UpdateRunAnimation();

    }

    void StopMovement()
    {
        velocity.x = 0f;
        velocity.z = 0f;
    }

    void SetNewWanderTarget()
    {
        // 20% 확률로 휴식
        if (Random.value < 0.2f)
        {
            isResting = true;
            restTimer = Random.Range(1.5f, 3f); // 1.5~3초간 휴식
            velocity = Vector3.zero;
            animator.SetFloat("Run", 0f); // 애니메이션 정지
            Debug.Log("[Enemy] 배회 중 잠시 휴식");
            return;
        }

        isResting = false;

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

        animator.SetTrigger("Attack");


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

        StopMovement(); // 이동 정지
        animator.SetTrigger("Death");

        // 공격 및 AI 중단
        currentState = default;
        player = null;

        // 아이템 드랍 → 애니메이션 끝나고 실행
        EnemyStats enemyStats = stats as EnemyStats;
        if (enemyStats != null && enemyStats.dropItem != null)
        {
            StartCoroutine(HandleDeathSequence(enemyStats.dropItem));
        }
        else
        {
            StartCoroutine(HandleDeathSequence(null));
        }
    }

    private IEnumerator HandleDeathSequence(DropItemData dropData)
    {
        float deathAnimDuration = 1.2f;
        yield return new WaitForSeconds(deathAnimDuration);

        if (dropData != null)
        {
            TryDropItem(dropData);
        }

        // ✅ 부활 요청
        EnemyRespawner respawner = FindObjectOfType<EnemyRespawner>();
        if (respawner != null)
        {
            respawner.StartRespawn(spawnPoint);
        }
        else
        {
            Debug.LogWarning("⚠ EnemyRespawner가 씬에 없음!");
        }

        Destroy(gameObject); // 🔥 사망 후 삭제
    }


    private void RespawnEnemy()
    {
        Debug.Log("[Enemy] 부활 처리 시작");

        transform.position = spawnPoint;
        currentHealth = stats.maxHealth;
        isDead = false;

        // 상태 초기화
        currentState = EnemyState.Wander;
        SetNewWanderTarget();

        // 애니메이션 초기화
        animator.ResetTrigger("Death");
        animator.Play("Idle"); // 또는 기본 상태명으로 변경

        // 다시 AI 활성화
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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

    void UpdateRunAnimation()
    {
        float moveAmount = new Vector3(velocity.x, 0f, velocity.z).magnitude / EnemyStats.moveSpeed;
        animator.SetFloat("Run", moveAmount);
    }

}
