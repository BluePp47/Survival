using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool startNotice = false; // 시작 알림창
    public List<InventoryItemSaveData> inventoryItems = new List<InventoryItemSaveData>();
}


[System.Serializable]
public class InventoryItemSaveData
{
    public string itemID;     // ItemData의 고유 ID
    public int quantity;
    public bool isEquipped;
}

