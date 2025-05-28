using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseCharacterController : MonoBehaviour
{
    public BaseStats stats;
    protected CharacterController characterController;
    protected Vector3 velocity;
    protected bool isGrounded;
    protected float gravity = -9.81f;

    protected int currentHealth;
    protected bool isDead = false;


    protected Animator animator;




    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = stats.maxHealth;
    }

    protected virtual void Update()
    {
        if (isDead) return;
        ApplyGravity();
    }

    protected void ApplyGravity()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            if (animator != null)
                animator.SetBool("Jump", false);

            if (this is PlayerController pc && pc.justJumped)
            {
                pc.justJumped = false;
            }
            else
            {
                velocity.y = -0.1f;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public virtual void TakeDamage(int attackerPower)
    {
        if (isDead) return;

        float defensePercent = stats.defense / (stats.defense + 100f);
        int damage = Mathf.RoundToInt(attackerPower * (1f - defensePercent));

        currentHealth -= damage;
        Debug.Log($"[{stats.characterType}] 데미지 받음: {damage}, 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        velocity = Vector3.zero;
        Debug.Log($"[{stats.characterType}] 사망 처리됨");

        // 🔄 파생 클래스에서 필요한 추가 처리를 위해 가상 메서드로 둠
        // 예: Destroy(gameObject); or 애니메이션 재생
    }
}
