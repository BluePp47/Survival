using UnityEngine;
using UnityEngine.UI;

// npc 호감도 - 하트
public class Heart : MonoBehaviour
{
    [SerializeField] Image[] heartImages; // 하트 이미지 3개
    public float health;

    public void SetHealth(float health) // 하트 반개당 0.5 => 즉, health = 1.5를 넣었을 때 하트 3개 중 1.5개가 채워짐. 
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            float value = Mathf.Clamp(health - i, 0, 1);
            heartImages[i].fillAmount = value;
        }
    }
}
