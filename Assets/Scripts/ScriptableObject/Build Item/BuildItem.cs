using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildMaterialRequirement
{
    public ItemData resourceItem;  // 나무, 돌 같은 자원
    public int requiredAmount;
}



[CreateAssetMenu(menuName = "Building/Build Item")]
public class BuildItem : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public Sprite icon;
    public string itemDescription;

    public List<BuildMaterialRequirement> materialRequirements;

    public BuildItemDatabase buildItemDatabase; // 제작된 아이템에 대한 정보 (optional)


}
