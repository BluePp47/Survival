using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseCharacterController : MonoBehaviour
{
    public BaseStats stats;
    protected CharacterController characterController;
    protected Vector3 velocity;
    protected bool isGrounded;
    protected float gravity = -9.81f;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        ApplyGravity();
    }

    protected void ApplyGravity()
    {
        isGrounded = characterController.isGrounded;

        // ✅ 항상 약간의 y값이 있도록 조정
        if (isGrounded && velocity.y < 0)
        {
            if (this is PlayerController pc && pc.justJumped)
            {
                pc.justJumped = false;
            }
            else
            {
                velocity.y = -0.1f;  // 👈 이걸 반드시 유지해야 controller.Move()가 작동함
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // ✅ 항상 Move 호출 (Enemy도 이 타이밍에서 움직이기 때문에 중요)
        characterController.Move(velocity * Time.deltaTime);
    }



    public virtual void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(0, damage - stats.defense);
        Debug.Log($"[{stats.characterType}] 데미지 받음: {finalDamage}");
        // 체력 감소는 외부에서 처리할 수도 있음
    }
}
