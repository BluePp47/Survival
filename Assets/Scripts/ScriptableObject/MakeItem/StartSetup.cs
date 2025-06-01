using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetup : MonoBehaviour
{
    void Start()
    {
        SaveData data = SaveSystem.LoadGame();
        GameObject.Find("Inventory").GetComponent<Inventory>().LoadInventory(data.inventoryItems);
        foreach (var item in data.inventoryItems)
        {
        Debug.Log($"[불러옴] {item.itemID}, 수량: {item.quantity}");
        }


        Time.timeScale = 1.0f;
        
        // UIController.Instance.CloseAllUI();
        // UIController.Instance.OpenUI("Condition");
    }

}
