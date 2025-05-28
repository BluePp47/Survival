using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    public float curValue;
    public float startValue = 100f;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;



    // Start is called before the first frame update
    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // ui업데이트 
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
        curValue = Mathf.Max(curValue - value, 0);
    }
}
