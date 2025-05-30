using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI equippedText;

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
        if (item == null || icon == null || item.icon == null) return;

        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);

        quantityText.text = (item.type != ItemType.Equipable && quantity > 1) ? quantity.ToString() : string.Empty;
        equippedText?.gameObject.SetActive(item.type == ItemType.Equipable && equipped);

        if (outline != null)
            outline.enabled = equipped;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
        equipped = false;

        icon?.gameObject.SetActive(false);
        quantityText.text = string.Empty;
        equippedText?.gameObject.SetActive(false);

        if (outline != null)
            outline.enabled = false;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
