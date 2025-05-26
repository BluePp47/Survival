using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class PlayerController : BaseCharacterController
{

    [Header("체력 관련")]
    public Slider healthSlider;
    public DamageFlashUI damageFlashUI;
    public float invincibleTime = 1.5f;

    [Header("스태미나 관련")]
    public Slider staminaSlider;


    private bool isInvincible = false;
    

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



    protected override void Awake()
    {
        base.Awake();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;

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
        currentStamina = PlayerStats.stamina;

        UpdateStaminaUI();
        UpdateHealthUI();
    }

    void OnDisable() => inputActions.Disable();

    protected override void Update()
    {
        base.Update();
        HandleMovement();
        RegenerateStamina();
        UpdateStaminaUI();
    }

    void HandleMovement()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            float speedMultiplier = 1f;

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

            // ✅ x, z 방향 속도를 velocity에 직접 반영
            velocity.x = moveDir.normalized.x * speed;
            velocity.z = moveDir.normalized.z * speed;
        }
        else
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }
    }


    void RegenerateStamina()
    {
        if (!isSprinting && currentStamina < PlayerStats.stamina)
        {
            currentStamina += PlayerStats.staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, PlayerStats.stamina);
        }
    }

    /// ✅ 점프 실행
    void TryJump()
    {
        if (currentStamina >= jumpStaminaCost)
        {
            currentStamina -= jumpStaminaCost;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            justJumped = true;
            Debug.Log("🟩 점프! (스태미나 소모)");
        }
        else
        {
            Debug.Log("❌ 점프 불가 - 스태미나 부족");
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

        float defensePercent = stats.defense / (stats.defense + 100f);
        int damage = Mathf.RoundToInt(attackerPower * (1f - defensePercent));

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
        Debug.Log("🗡️ TryBasicAttack() 호출됨");
        Vector3 attackOrigin = transform.position + transform.forward * attackRange * 0.5f;
        Collider[] hitTargets = Physics.OverlapSphere(attackOrigin, attackRadius, attackLayerMask);

      

        foreach (Collider target in hitTargets)
        {
            BaseCharacterController enemy = target.GetComponent<BaseCharacterController>();
            if (enemy != null && enemy != this)
            {
                enemy.TakeDamage(stats.attackPower);
                Debug.Log($"[Attack] {target.name}에게 {stats.attackPower} 피해!");
              

                // ✅ 이펙트 생성 후 0.5초 뒤 삭제
                if (hitEffectPrefab != null)
                {
                    Vector3 hitPos = target.ClosestPoint(transform.position);
                    GameObject fx = Instantiate(hitEffectPrefab, hitPos, Quaternion.identity);
                    Destroy(fx, 0.5f);
                }
            }
        }

      
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 attackOrigin = transform.position + transform.forward * attackRange * 0.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }

}
