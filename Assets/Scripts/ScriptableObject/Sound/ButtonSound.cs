using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public void PlayClickSound()
    {
        SoundEvents.OnUIClick?.Invoke();
    }
}