using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;


public class PlayerController : BaseCharacterController, IInventoryHolder
{
    [SerializeField] private AudioClip[] attackAudio;
    [SerializeField] private AudioClip jumpAudio;

    [SerializeField] private GameObject dustParticlePrefab;
    [SerializeField] private Transform dustSpawnPoint; // 발 위치 또는 지면 기준

    private float walkingTimer = 0f;
    private float dustSpawnCooldown = 0f;
    private float walkThresholdTime = 0.5f;

    [SerializeField] private float dustSpawnInterval = 0.5f;


    public Inventory inventory;
    public UIInventory uiInventory;
    public PlayerCondition condition;
    public Transform dropPosition;

    public ItemData itemData;
    public Action onInventoryToggle;
    public Action addItem;



    public Inventory GetInventory()
    {
        return inventory;
    }

    [Header("체력 관련")]
    public Slider healthSlider;
    public DamageFlashUI damageFlashUI;
    public float invincibleTime = 1.5f;

    [Header("배고픔 관련")]
    public Image hungerFillImage; 
    public float maxHunger = 100f;
    public float currentHunger = 100f;
    public float hungerDecreaseRate = 2f; 
    
    [Header("목마름 관련")]
    public Slider thirstSlider; 
    public float maxThirst = 100f;  
    public float currentThirst = 100f; 
    public float thirstDecreaseRate = 1.0f;
    
    [Header("스태미나 관련")]
    public Slider staminaSlider;


    private bool isInvincible = false;


    private bool isBlocking = false;
    [SerializeField] private float blockStaminaCost = 20f;
    [SerializeField] private float blockDamageReduction = 0.5f;



    public Transform cameraTransform;
    public float rotationSpeed = 10f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isSprinting = false;

    [Header("점프 설정")]
    public float jumpHeight = 3f;
    public float jumpStaminaCost = 15f;

    public bool justJumped = false;

    private float currentStamina;
    private PlayerStats PlayerStats => (PlayerStats)stats;

    [SerializeField] private float attackRange = 2f;      // 전방 공격 거리
    [SerializeField] private float attackRadius = 1f;     // 원형 범위
    [SerializeField] private LayerMask attackLayerMask;   // 공격 대상 레이어 (예: Enemy)


    [Header("공격 이펙트")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Transform hitEffectSpawnPoint;


    private float attackCooldownTimer = 0f;
    [SerializeField] private float attackInterval = 0.7f; // ⏱ 공격 간격



    protected override void Awake()
    {
        base.Awake();
        inputActions = new PlayerInputActions();
        animator = GetComponent<Animator>();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;
        inputActions.Player.Block.started += _ =>
        {
            isBlocking = true;
            animator.SetBool("Df", true); // 🔹 막기 애니메이션 활성화
        };

        inputActions.Player.Block.canceled += _ =>
        {
            isBlocking = false;
            animator.SetBool("Df", false); // 🔹 막기 애니메이션 비활성화
        };



        inputActions.Player.Jump.performed += _ =>
        {
            if (CanJump())
                TryJump();
        };
        inputActions.Player.Attack.performed += _ => TryBasicAttack();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Inventory.performed += ctx => onInventoryToggle?.Invoke();
        currentStamina = PlayerStats.stamina;
        currentHunger = maxHunger;
        currentThirst = maxThirst;

        UpdateStaminaUI();
        UpdateHealthUI();
        UpdateHungerUI();
        UpdateThirstUI();
    }

    void OnDisable() => inputActions.Disable();

    protected override void Update()
    {
        base.Update();
        HandleMovement();
        RegenerateStamina();
        
        UpdateStaminaUI();
        
        DecreaseHunger();
        UpdateHungerUI();
        
        DecreaseThirst();
        UpdateThirstUI();
        
        attackCooldownTimer -= Time.deltaTime;
        
    }
    void HandleMovement()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        float moveAmount = direction.magnitude;

        float speedMultiplier = 1f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); // 즉시 회전

           

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (isSprinting)
            {
                if (currentStamina > 0f)
                {
                    speedMultiplier = 1.5f;
                    currentStamina -= 20f * Time.deltaTime;
                    currentStamina = Mathf.Max(currentStamina, 0f);
                }
                else
                {
                    isSprinting = false;
                }
            }

            float speed = stats.moveSpeed * speedMultiplier;

            if (isBlocking)
            {
                speed *= 0.7f; // 막기 상태일 때 이동 속도 70% 감소
            }

            velocity.x = moveDir.normalized.x * speed;
            velocity.z = moveDir.normalized.z * speed;
        }
        else
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        


        animator.SetFloat("Run", moveAmount * speedMultiplier);
        animator.SetBool("Sprint", isSprinting && !isBlocking);


        bool isMovingOnGround = moveAmount > 0.1f && isGrounded;

        if (isMovingOnGround)
        {
            walkingTimer += Time.deltaTime;
            dustSpawnCooldown -= Time.deltaTime;

            if (walkingTimer >= walkThresholdTime && dustSpawnCooldown <= 0f)
            {
                SpawnDustParticle();
                dustSpawnCooldown = dustSpawnInterval;
            }
        }
        else
        {
            walkingTimer = 0f;
            dustSpawnCooldown = 0f;
        }



    }

    void DecreaseHunger() //배고픔
    {
        if (currentHunger > 0f)
        {
            currentHunger -= hungerDecreaseRate * Time.deltaTime;
            currentHunger = Mathf.Max(currentHunger, 0f);
        }
    }
    
    void UpdateHungerUI()
    {
        if (hungerFillImage == null) return;

        float percent = currentHunger / maxHunger;
        hungerFillImage.fillAmount = percent;
    }
    
    void DecreaseThirst() //목마름
    {
        if (currentThirst > 0f)
        {
            currentThirst -= thirstDecreaseRate * Time.deltaTime;
            currentThirst = Mathf.Max(currentThirst, 0f);
        }
    }

// 슬라이더 UI 업데이트
    void UpdateThirstUI()
    {
        if (thirstSlider == null) return;

        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = Mathf.Clamp(currentThirst, 0f, maxThirst);
    }

    void RegenerateStamina()
    {
        if (!isSprinting && currentStamina < PlayerStats.stamina)
        {
            currentStamina += PlayerStats.staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, PlayerStats.stamina);
        }
    }

    void TryJump()
    {
        if (isBlocking)
        {
            Debug.Log("방어 중에는 점프할 수 없습니다");
            return;
        }

        if (currentStamina >= jumpStaminaCost)
        {
            currentStamina -= jumpStaminaCost;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            justJumped = true;

            animator.SetBool("Jump", true);
            SoundEvents.OnPlaySFX?.Invoke(jumpAudio);


            Debug.Log("점프! (스태미나 소모)");
        }
        else
        {
            Debug.Log("점프 불가 - 스태미나 부족");
        }
    }


    /// ✅ 점프 가능 여부 (지면 또는 거의 붙은 상태)
    bool CanJump()
    {
        return characterController.isGrounded || IsAlmostGrounded();
    }

    /// ✅ 캐릭터가 거의 바닥에 있는 상태인지 체크
    bool IsAlmostGrounded()
    {
        return !characterController.isGrounded && velocity.y > -0.2f && velocity.y <= 0f;
    }

    public override void TakeDamage(int attackerPower)
    {
        if (isDead || isInvincible) return;

        float finalDamage = attackerPower;

        if (isBlocking && currentStamina >= blockStaminaCost)
        {
            finalDamage *= (1f - blockDamageReduction); // 50% 피해 감소
            currentStamina -= blockStaminaCost;
            currentStamina = Mathf.Max(0f, currentStamina);
            Debug.Log($"막기 성공! 피해 감소됨. 남은 스태미나: {currentStamina}");
        }
        else
        {
            Debug.Log("⚠막기 실패 또는 스태미나 부족 → 피해 그대로 적용");
        }

        float defensePercent = stats.defense / (stats.defense + 100f);
        int damage = Mathf.RoundToInt(finalDamage * (1f - defensePercent));

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (damageFlashUI != null)
            damageFlashUI.Flash();

        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(InvincibilityCoroutine());
    }


    private void UpdateHealthUI()
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = stats.maxHealth;  // ✅ maxHealth는 stats에서 가져옴
        healthSlider.value = Mathf.Clamp(currentHealth, 0, stats.maxHealth);
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider == null) return;

        staminaSlider.maxValue = PlayerStats.stamina;
        staminaSlider.value = Mathf.Clamp(currentStamina, 0f, PlayerStats.stamina);
    }
    

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Death"); // 🔹 사망 애니메이션 실행
        Debug.Log("[Player] 사망 - 게임 오버 처리 시작");
        StartCoroutine(DeathSequence());
    }


    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1f);

        if (FadeManager.Instance != null)
            FadeManager.Instance.FadeToScene("GameOver");
        else
            SceneManager.LoadScene("GameOver");
    }


    void TryBasicAttack()
    {
        if (attackCooldownTimer > 0f)
        {
            Debug.Log("공격 쿨타임 중...");
            return;
        }

        if (isSprinting)
        {
            Debug.Log("스프린트 중엔 공격 불가");
            return;
        }

       

        animator.SetTrigger("Attack");
        Invoke(nameof(PlayDelayedAttackAudio), 0.45f);
        attackCooldownTimer = attackInterval;
        Debug.Log("공격 실행");

        Vector3 attackOrigin = transform.position + transform.forward * attackRange * 0.5f;
        Collider[] hitTargets = Physics.OverlapSphere(attackOrigin, attackRadius, attackLayerMask);

        foreach (Collider target in hitTargets)
        {
            BaseCharacterController enemy = target.GetComponent<BaseCharacterController>();
            if (enemy != null && enemy != this)
            {
                enemy.TakeDamage(stats.attackPower);
                Debug.Log($"[Attack] {target.name}에게 {stats.attackPower} 피해!");

                if (hitEffectPrefab != null)
                {
                    Vector3 hitPos = target.ClosestPoint(transform.position);
                    GameObject fx = Instantiate(hitEffectPrefab, hitPos, Quaternion.identity);
                    Destroy(fx, 0.5f);
                }
            }

            Resource resource = target.GetComponent<Resource>();
            if (resource != null)
            {
                Vector3 hitPoint = target.ClosestPoint(transform.position);
                Vector3 hitNormal = (target.transform.position - transform.position).normalized;
                resource.Gather(hitPoint, hitNormal);
            }
        }
    }


    public void AddItemToInventory(ItemData item)
    {
        this.itemData = item;
        addItem?.Invoke(); 
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 attackOrigin = transform.position + transform.forward * attackRange * 0.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }

    void SpawnDustParticle()
    {
        if (dustParticlePrefab == null || dustSpawnPoint == null) return;

        GameObject dust = Instantiate(dustParticlePrefab, dustSpawnPoint.position, Quaternion.identity);
        Destroy(dust, 1f);
    }

    void PlayDelayedAttackAudio()
    {
        SoundEvents.OnPlaySFX2?.Invoke(attackAudio);
    }

}
