using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
<<<<<<< HEAD
=======
    private AudioSource audioSource;
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    private Rigidbody _rigidbody;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
<<<<<<< HEAD
=======
        audioSource = GetComponent<AudioSource>();
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    }

    private void Update()
    {
        if(Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            if(_rigidbody.velocity.magnitude > footstepThreshold)
            {
                if(Time.time - footStepTime > footstepRate)
                {
                    footStepTime = Time.time;
<<<<<<< HEAD
                    SoundEvents.OnPlaySFX2?.Invoke(footstepClips);
=======
                    // SoundEvents.OnPlaySFX?.Invoke(footstepClips);
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
                }
            }
        }
    }

}
