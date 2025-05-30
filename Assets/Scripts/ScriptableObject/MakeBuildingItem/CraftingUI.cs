using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    // Build Item 불러오기
    public BuildItemDatabase buildDatabase;
    public GameObject itemUIPrefab;
    public Transform contentParent;
    
    //지금 선택한 아이템
    private BuildItem currentSelectedItem;
    
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
        // buildItem UI에 보여주기. 
        foreach (BuildItem item in buildDatabase.buildItems)
        {
            GameObject uiObj = Instantiate(itemUIPrefab, contentParent);
            MakebuildItemUI ui = uiObj.GetComponent<MakebuildItemUI>();
            ui.Setup(item, SelectItem);
        }
    }

    private void SelectItem(BuildItem item)
    {
        currentSelectedItem = item;

        selectedItemName.text = item.itemName;
        selectedItemDescription.text = item.itemDescription; // item.description이 있으면 더 좋음
        selectedItemMeterial_image.sprite = item.icon;
        selectedItemMeterial_name.text = "목재 + 석재"; // 혹은 재료 이름 로직으로
        selectedItemMeterial_count.text = $"{item.woodCost} + {item.stoneCost}";
    }

    // 만들기 버튼 클릭
    private void OnMakeButtonClicked()
    {
        if (currentSelectedItem != null)
                Invoke("TryBuildItem(currentSelectedItem)", 1);
    }

    // 재료 소모
    private void TryBuildItem(BuildItem item)
    {
        if (HasResources(item))
        {
            SpendResources(item);
            Instantiate(item.prefab, Vector3.zero, Quaternion.identity); // 원하는 위치로 수정
            Debug.Log("만들었습니다");
        }
        else
        {
            Debug.Log("Not enough resources!");
        }
    }

    private bool HasResources(BuildItem item)
    {
        // ResourceManager와 연동 필요 (임시로 true 반환)
        return true;
    }

    private void SpendResources(BuildItem item)
    {
        // ResourceManager에서 자원 차감
        Debug.Log("spendResources");
    }
    
    //     public void AddItem()
    // {
    //     ItemData data = CharacterManager.Instance.Player.itemData;

    //     if (data.canStack)
    //     {
    //         ItemSlot slot = GetItemStack(data);
    //         if(slot != null)
    //         {
    //             slot.quantity++;
    //             UpdateUI();
    //             CharacterManager.Instance.Player.itemData = null;
    //             return;
    //         }
    //     }

}
