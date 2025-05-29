using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
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
        Debug.Log($"[ItemSlot] Set() 호출됨 - item: {(item != null ? item.name : "null")}");

        if (item == null)
        {
            Debug.LogWarning($"[ItemSlot] item == null → Set 중단. index: {index}");
            return;
        }

        if (item.icon == null)
        {
            Debug.LogError($"[ItemSlot] item.icon == null → 아이콘 없음. item: {item.name}");
            return;
        }

        if (icon == null)
        {
            Debug.LogError($"[ItemSlot] icon UI 연결 안됨");
            return;
        }

        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        Debug.Log($"[ItemSlot] Set() 완료: {item.name} x{quantity}");
    }


    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}