using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStats : BaseStats
{
    [Header("플레이어 전용 스탯")]
    public float jumpHeight = 3f;

    public float stamina = 100f;
    public float staminaRegenRate = 5f;

    public float hunger = 100f;
    public float thirst = 100f;

    public float hungerDecayRate = 1f;
    public float thirstDecayRate = 1.5f;
}
