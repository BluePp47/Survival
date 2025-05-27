using UnityEngine;
<<<<<<< HEAD
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
=======

public class UIButtonSound : MonoBehaviour
{
    public void PlayClickSound()
    {
        SoundEvents.OnUIClick?.Invoke();
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    }
}