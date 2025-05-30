using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class DeskinProduction : MonoBehaviour
{
    [SerializeField] GameObject deskinProductionUI;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            deskinProductionUI.SetActive(true);
            ToggleCursor();
        }
    }

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
    
}
