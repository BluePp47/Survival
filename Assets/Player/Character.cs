using UnityEngine;

public class Character : MonoBehaviour
{
    public BaseStats stats;

    private int currentHealth;

    void Start()
    {
        currentHealth = stats.maxHealth;
        Debug.Log($"[{stats.characterType}] 체력: {currentHealth}, 공격력: {stats.attackPower}");
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(0, damage - stats.defense);
        currentHealth -= finalDamage;
        Debug.Log($"데미지 받음: {finalDamage}, 현재 체력: {currentHealth}");
    }
}
