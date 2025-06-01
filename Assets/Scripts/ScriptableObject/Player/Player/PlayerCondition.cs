using UnityEngine;
using UnityEngine.UI;

public enum ConditionType { Health, Hunger, Thirst, Stamina }

public class PlayerCondition : MonoBehaviour
{
    public ConditionType type;
    public float startValue = 100f;
    public float maxValue = 100f;
    public float passiveChangePerSecond = 0f;
    public Image uiBar;

    [HideInInspector]
    public float curValue;

    void Start()
    {
        curValue = startValue;
    }

    void Update()
    {
        if (passiveChangePerSecond != 0f)
        {
            curValue += passiveChangePerSecond * Time.deltaTime;
            curValue = Mathf.Clamp(curValue, 0f, maxValue);
        }

        if (uiBar != null)
            uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        float oldValue = curValue;
        curValue = Mathf.Max(curValue - value, 0);
    }

}
