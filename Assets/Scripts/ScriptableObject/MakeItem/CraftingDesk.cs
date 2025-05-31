using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingDesk : MonoBehaviour
{
    [SerializeField] GameObject deskinProductionUI;

    void ToggleCursor()
    {
        bool isLocked = Cursor.lockState == CursorLockMode.Locked;

        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // canLook = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // canLook = true;
        }
    }

    public void Interact()
    {
        deskinProductionUI.SetActive(true);
        ToggleCursor();
    }
}

    
