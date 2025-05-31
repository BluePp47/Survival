using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        UIController.Instance.CloseAllUI();
        UIController.Instance.OpenUI("Condition");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
