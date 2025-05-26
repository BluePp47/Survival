using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EnemyStats")]
public class EnemyStats : BaseStats
{
    [Header("적 전용 스탯")]
    public float attackInterval = 1.5f;      // 공격 간격
    public float detectionRange = 10f;       // 감지 거리
    public float returnDistance = 15f;       // 추적 포기 거리
    public float attackRange = 2f;           // 공격 사정거리
    [Header("드랍 아이템")]
    public DropItemData dropItem;
}
