using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseCharacterController
{
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
    }

    void OnEnable()
    {
        inputActions.Enable();
        currentStamina = PlayerStats.stamina;
    }

    void OnDisable() => inputActions.Disable();

    protected override void Update()
    {
        base.Update();
        HandleMovement();
        RegenerateStamina();
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
}
