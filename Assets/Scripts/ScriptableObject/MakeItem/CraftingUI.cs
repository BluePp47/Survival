using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public BuildItemDatabase buildDatabase;
    public GameObject itemUIPrefab;
    public Transform contentParent;

    private void Start()
    {
        foreach (var item in buildDatabase.buildItems)
        {
            GameObject uiObj = Instantiate(itemUIPrefab, contentParent);
            BuildItemUI ui = uiObj.GetComponent<BuildItemUI>();
            ui.Setup(item, TryBuildItem);
        }
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
