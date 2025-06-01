using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    // Build Item 불러오기
    public BuildItemDatabase buildDatabase;
    public GameObject itemUIPrefab;
    public Transform contentParent;

    // 선택한 아이템의 이름/설명
    BuildItem currentSelectedItem;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    
    // 필요한 재료의 이미지/이름/갯수
    public Image selectedItemMaterial_image;
    public TextMeshProUGUI selectedItemMaterial_name;
    public TextMeshProUGUI selectedItemMaterial_count;

    // 만들기 버튼
    public Button MakeButton;

    Inventory playerInventory;
    public Transform materialListParent;

    public GameObject materialSlotPrefab;    private void Start()
    {
        foreach (var item in buildDatabase.buildItems)
        {
            GameObject uiObj = Instantiate(itemUIPrefab, contentParent);
            BuildItemSlot ui = uiObj.GetComponent<BuildItemSlot>();
            playerInventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            ui.Setup(item, SelectItem);
        }

        MakeButton.onClick.AddListener(OnMakeButtonClicked);
    }

    private void SelectItem(BuildItem item)
    {
        currentSelectedItem = item;

        selectedItemName.text = item.itemName;
        selectedItemDescription.text = item.itemDescription;
        //이전 UI 제거
        foreach (Transform child in materialListParent)
                Destroy(child.gameObject);

            // 새로운 재료 목록 표시
            foreach (var req in item.materialRequirements)
            {
                var slot = Instantiate(materialSlotPrefab, materialListParent);
                Image icon = slot.transform.Find("Icon_material").GetComponent<Image>();
                TextMeshProUGUI name = slot.transform.Find("Name_material").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI number = slot.transform.Find("number_material").GetComponent<TextMeshProUGUI>();

                icon.sprite = req.resourceItem.icon;
                name.text = req.resourceItem.displayName;
                int currentAmount = ResorceManager.Instance.GetAmount(req.resourceItem);
                number.text = $"{currentAmount} / {req.requiredAmount}";
        }
    // selectedItemMeterial_image.sprite = item.icon;
    // selectedItemMeterial_name.text = "목재 + 석재";
    // selectedItemMeterial_count.text = $"0";
}


    private void TryBuildItem(BuildItem item)
    {
        if (HasResources(item))
        {
            SpendResources(item);
            Debug.Log("설계도에 추가되었습니다");
            
            // 설계도에 추가하기.  nstantiate(item.prefab, Vector3.zero, Quaternion.identity); // 원하는 위치로 수정
        }
        else
        {
            Debug.Log("Not enough resources!");
        }
    }

    private void OnMakeButtonClicked()
    {
        if (currentSelectedItem != null)
            Invoke("ShowItemNotice", 1f);
            TryBuildItem(currentSelectedItem);
    }


    private void ShowItemNotice()
    {
        NoticeUI.Instance.Show($"[{currentSelectedItem.itemName}]가/이 설계도에 추가되었습니다.", 1);
    }

    private bool HasResources(BuildItem item)
    {
        foreach (var req in item.materialRequirements)
        {
            if (!playerInventory.HasEnoughItem(req.resourceItem, req.requiredAmount))
                return false;
        }
        return true;
    }

    private void SpendResources(BuildItem item)
    {
        foreach (var req in item.materialRequirements)
        {
            playerInventory.SpendItem(req.resourceItem, req.requiredAmount);
        }
    }

}


