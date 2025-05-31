using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject interactableObject;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private TextMeshProUGUI interactObjectName;

    private void Start()
    {
        UIController.Instance.CloseAllUI();
    }

    public void OnInteraction()
    {
        if(interactableObject != null)
        {
            interactableObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CollisionObject(collision, "CraftingDesk", "제작대 열기");
        // CollisionObject(collision, "NPC", "NPC와 대화");
        CollisionObject(collision, "Door", "밖으로 나가기");
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("CraftingDesk") || 
            collision.gameObject.CompareTag("Door"))
        {
            interactableObject = null;
            interactUI.SetActive(false);
        }

        // if(collision.gameObject.CompareTag("NPC"))
        // {
        //     interactableObject = null;
        //     interactUI.SetActive(false);
        // }
    }

    void CollisionObject(Collision collision, string tagName, string description)
    {
        if(collision.gameObject.CompareTag(tagName))
        {
            interactableObject = collision.gameObject;
            interactObjectName.text = description;
            interactUI.SetActive(true);
        }

    }

}