using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    [HideInInspector] public int index;
    [HideInInspector] public UIInventory inventory;
    [HideInInspector] public ItemData item;
    [HideInInspector] public int quantity;
    [HideInInspector] public bool equipped;

   

    public void Set()
    {
        icon.sprite = item.icon;
        icon.enabled = true;

        quantityText.text = item.canStack ? quantity.ToString() : "";
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
        equipped = false;

        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }

    void OnClick()
    {
        inventory.SelectItem(index);
    }
}
