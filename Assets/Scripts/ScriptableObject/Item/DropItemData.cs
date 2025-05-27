using UnityEngine;

[CreateAssetMenu(fileName = "DropItemData", menuName = "Item/DropItem")]
public class DropItemData : ScriptableObject
{
    public GameObject itemPrefab;  // 드랍할 아이템 프리팹
    public int dropChance = 100;   // 드랍 확률 (%)
}
