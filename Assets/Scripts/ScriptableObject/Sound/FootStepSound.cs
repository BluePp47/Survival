// using UnityEngine;

// public class FootSteps : MonoBehaviour
// {
//     public AudioClip[] footstepClips;
//     private Rigidbody _rigidbody;
//     public float footstepThreshold;
//     public float footstepRate;
//     private float footStepTime;

//     private void Start()
//     {
//         _rigidbody = GetComponent<Rigidbody>();
//     }

//     private void Update()
//     {
//         if(Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
//         {
//             if(_rigidbody.velocity.magnitude > footstepThreshold)
//             {
//                 if(Time.time - footStepTime > footstepRate)
//                 {
//                     footStepTime = Time.time;
//                     SoundEvents.OnPlaySFX2?.Invoke(footstepClips);
//                 }
//             }
//         }
//     }

// }

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepSound : MonoBehaviour
{
    public float footstepThreshold = 0.1f;
    public AudioClip[] footstepClip;
    private CharacterController controller;
    private Vector3 lastPosition;
    private float stepCooldown = 0.4f;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        float speed = velocity.magnitude;

        stepTimer += Time.deltaTime;

        if (controller.isGrounded && speed > footstepThreshold && stepTimer > stepCooldown)
        {
            SoundEvents.OnPlaySFX2?.Invoke(footstepClip);
            stepTimer = 0f;
        }

        lastPosition = transform.position;
    }
}
