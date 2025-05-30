using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    // Build Item 불러오기
    public BuildItemDatabase buildDatabase;
    public GameObject itemUIPrefab;
    public Transform contentParent;

    BuildItem currentSelectedItem;

    // 선택한 아이템의 이름/설명
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    
    // 필요한 재료의 이미지/이름/갯수
    public Image selectedItemMeterial_image;
    public TextMeshProUGUI selectedItemMeterial_name;
    public TextMeshProUGUI selectedItemMeterial_count;

    // 만들기 버튼
    public Button MakeButton;

    private void Start()
    {
        foreach (var item in buildDatabase.buildItems)
        {
            GameObject uiObj = Instantiate(itemUIPrefab, contentParent);
            BuildItemSlot ui = uiObj.GetComponent<BuildItemSlot>();
            ui.Setup(item, SelectItem);
        }

        MakeButton.onClick.AddListener(OnMakeButtonClicked);
    }

    private void SelectItem(BuildItem item)
{
    currentSelectedItem = item;

    selectedItemName.text = item.itemName;
    selectedItemDescription.text = item.itemDescription;
    selectedItemMeterial_image.sprite = item.icon;
    selectedItemMeterial_name.text = "목재 + 석재";
    selectedItemMeterial_count.text = $"{item.woodCost} + {item.stoneCost}";
}


    private void TryBuildItem(BuildItem item)
    {
        if (HasResources(item))
        {
            SpendResources(item);
            Instantiate(item.prefab, Vector3.zero, Quaternion.identity); // 원하는 위치로 수정
        }
        else
        {
            Debug.Log("Not enough resources!");
        }
    }
    private void OnMakeButtonClicked()
    {
        if (currentSelectedItem != null)
            TryBuildItem(currentSelectedItem);
    }
    private bool HasResources(BuildItem item)
    {
        // ResourceManager와 연동 필요 (임시로 true 반환)
        return true;
    }

    private void SpendResources(BuildItem item)
    {
        // ResourceManager에서 자원 차감
    }
}
