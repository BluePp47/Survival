using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    GameObject npcWindow;
    
    public void CloseButtonPressed()
    {
        npcWindow.SetActive(false);
    }
}
