using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInterAction : MonoBehaviour
{
    public GameObject Dialog; 

    private bool wasClosedByPlayer = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!wasClosedByPlayer)
            {
                Debug.Log("active");
                Dialog.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("deactive");
            Dialog.SetActive(false);
            wasClosedByPlayer = false; 
        }
    }

    public void CloseDialogManually()
    {
        Dialog.SetActive(false);
        wasClosedByPlayer = true;
    }
}
