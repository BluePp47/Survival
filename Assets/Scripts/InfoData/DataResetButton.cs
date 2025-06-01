using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataResetButton : MonoBehaviour
{
    public void OnClick_ResetSaveData()
    {
        SaveSystem.ResetGame();
    }
}
    