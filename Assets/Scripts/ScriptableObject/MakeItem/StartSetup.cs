using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetup : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1.0f;
        
        SaveData data = SaveSystem.LoadGame();
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.LoadInventory(data.inventoryItems);
        foreach (var item in data.inventoryItems)
        {
        Debug.Log($"[불러옴] {item.itemID}, 수량: {item.quantity}");
        }
                
        UIController.Instance.CloseAllUI();
        UIController.Instance.OpenUI("Condition");
    }

}
