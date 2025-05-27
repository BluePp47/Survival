using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumption : MonoBehaviour
{
    public float healthRestore;
    public float staminaRestore;
    public float hungerRestore;
    public float thirstRestore;

    public void Use(GameObject user)
    {
        Player player = user.GetComponent<Player>();
        if (player != null)
        {
            if (healthRestore > 0) player.RestoreHealth(healthRestore);
            if (staminaRestore > 0) player.RestoreStamina(staminaRestore);
            if (hungerRestore > 0) player.RestoreHunger(hungerRestore);
            if (thirstRestore > 0) player.RestoreThirst(thirstRestore);
        }
    }
}
