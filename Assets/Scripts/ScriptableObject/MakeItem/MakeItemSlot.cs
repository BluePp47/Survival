using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    //private void OnEnable()
    //{
    //    outline.enabled = equipped;
    //}

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
