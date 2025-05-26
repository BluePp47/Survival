using UnityEngine;

[CreateAssetMenu(menuName = "Building/Build Item")]
public class BuildItem : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public Sprite icon;
    public int woodCost; // 필요 자원 비용 (예시 나무)
    public int stoneCost; // 필요 자원 비용 (예시 돌)
}
