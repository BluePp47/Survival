using UnityEngine;

[CreateAssetMenu(menuName = "Building/Build Item")]
public class BuildItem : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public Sprite icon;
    public string itemDescription;
    public int woodCost; // �ʿ� �ڿ� ��� (���� ����)
    public int stoneCost; // �ʿ� �ڿ� ��� (���� ��)
}
