using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundEvents.OnUIClick?.Invoke();
        });      
    }
}