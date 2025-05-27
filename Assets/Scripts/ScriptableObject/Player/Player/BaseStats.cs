using UnityEngine;

public enum CharacterTag { Player, Enemy, Resource }

[CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
public class BaseStats : ScriptableObject
{
    [Header("공통 스탯")]
    public CharacterTag characterType;
    public int maxHealth;
    public int attackPower;
    public int defense;
    public float moveSpeed;

    // 필요하다면 더 많은 스탯 추가 가능
}
