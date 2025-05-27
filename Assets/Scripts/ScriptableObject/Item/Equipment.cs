using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData equippedItem;

    public void Equip(ItemData item)
    {
        if (item.type != ItemType.Equipable) return;
        equippedItem = item;
    }

    public void Unequip()
    {
        equippedItem = null;
    }
}
