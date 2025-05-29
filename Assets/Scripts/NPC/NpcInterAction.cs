using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcInterAction : MonoBehaviour
{
    public GameObject Dialog;  // 열고 닫을 UI 오브젝트

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player")) 
        {
            Debug.Log("active");
            Dialog.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            Debug.Log("deactive");
            Dialog.SetActive(false);
        }
    }
}