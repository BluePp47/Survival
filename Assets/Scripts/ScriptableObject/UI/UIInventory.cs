using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;

    [Header("Inventory Slots")]
    public ItemSlot[] slots;
    public Transform slotPanel;

    [Header("UI Panels")]
    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    private ItemData selectedItem;
    private int selectedItemIndex = -1;

    void Start()
    {
        StartCoroutine(InitUI());
    }

    IEnumerator InitUI()
    {
        yield return new WaitUntil(() => CharacterManager.Instance.Player != null);

        controller = CharacterManager.Instance.Player;
        controller.onInventoryToggle += Toggle;

        inventory = controller.GetInventory();
        condition = controller.condition;
        dropPosition = controller.dropPosition;

        inventoryWindow.SetActive(false);

        // Initialize slots
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    public void RefreshUI(List<InventoryItem> inventoryItems)
    {
        Debug.Log($"[UI] 받은 아이템 수: {inventoryItems.Count}");

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryItems.Count)
            {
                InventoryItem invItem = inventoryItems[i];

                if (invItem == null || invItem.data == null)
                {
                    Debug.LogError($"[UI] 슬롯 {i}에 잘못된 데이터");
                    continue;
                }

                slots[i].item = invItem.data;
                slots[i].quantity = invItem.quantity;
                slots[i].index = i;
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void Toggle()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeInHierarchy);
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        selectedStatName.text = "";
        selectedStatValue.text = "";

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = "";
        selectedStatValue.text = "";

        foreach (var stat in selectedItem.consumables)
        {
            selectedStatName.text += stat.type + "\n";
            selectedStatValue.text += stat.value + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem == null || selectedItem.type != ItemType.Consumable) return;

        foreach (var stat in selectedItem.consumables)
        {
            switch (stat.type)
            {
                case ConsumableType.Health:
                case ConsumableType.Hunger:
                case ConsumableType.Thirsty:
                    condition.Add(stat.value);
                    break;
            }
        }

        RemoveSelectedItem();
    }

    public void OnDropButton()
    {
        if (selectedItem != null)
        {
            ThrowItem(selectedItem);
            RemoveSelectedItem();
        }
    }

    void RemoveSelectedItem()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= slots.Length) return;

        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            slots[selectedItemIndex].item = null;
            selectedItem = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                slots[i].Set();
            else
                slots[i].Clear();
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        foreach (var slot in slots)
        {
            if (slot.item == data && slot.quantity < data.maxStackAmount)
                return slot;
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot.item == null)
                return slot;
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        if (data.dropPrefab != null && dropPosition != null)
            Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 300));
    }
}
