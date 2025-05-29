using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("UI")]
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI equippedText;

    [Header("Data")]
    public UIInventory inventory;
    public ItemData item;
    public int index;
    public int quantity;
    public bool equipped;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Set()
    {
        if (item == null || item.icon == null || icon == null) return;

        // 아이콘 표시
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        // 수량 표시 조건: 장비가 아닐 때만
        if (quantityText != null)
        {
            bool showQuantity = item.type != ItemType.Equipable;
            quantityText.text = (showQuantity && quantity > 1) ? quantity.ToString() : string.Empty;
        }

        // 장비일 때만 E 표시
        if (equippedText != null)
        {
            bool showE = (item.type == ItemType.Equipable && equipped);
            equippedText.gameObject.SetActive(showE);
        }
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
        equipped = false;

        if (icon != null)
            icon.gameObject.SetActive(false);

        if (quantityText != null)
            quantityText.text = string.Empty;

        if (equippedText != null)
            equippedText.gameObject.SetActive(false);
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
