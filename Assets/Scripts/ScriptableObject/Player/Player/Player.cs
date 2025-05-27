using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IInventoryHolder
{
    public Inventory inventory;
    public PlayerStats baseStats;

    public float currentHealth;
    public float currentStamina;
    public float currentHunger;
    public float currentThirst;

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void Start()
    {
        currentHealth = baseStats.maxHealth;
        currentStamina = baseStats.stamina;
        currentHunger = baseStats.hunger;
        currentThirst = baseStats.thirst;
    }

    private void Update()
    {
        currentStamina = Mathf.Clamp(currentStamina + baseStats.staminaRegenRate * Time.deltaTime, 0, baseStats.stamina);
        currentHunger = Mathf.Clamp(currentHunger - baseStats.hungerDecayRate * Time.deltaTime, 0, baseStats.hunger);
        currentThirst = Mathf.Clamp(currentThirst - baseStats.thirstDecayRate * Time.deltaTime, 0, baseStats.thirst);
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, baseStats.maxHealth);
    }

    public void RestoreStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, baseStats.stamina);
    }

    public void UseStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, baseStats.stamina);
    }

    public void RestoreHunger(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, baseStats.hunger);
    }

    public void RestoreThirst(float amount)
    {
        currentThirst = Mathf.Clamp(currentThirst + amount, 0, baseStats.thirst);
    }
}

