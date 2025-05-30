using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceManager : MonoBehaviour
{
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int wood;
    public int stone;

    private void Awake() => Instance = this;

    public bool HasEnough(int woodCost, int stoneCost) =>
        wood >= woodCost && stone >= stoneCost;

    public void Spend(int woodCost, int stoneCost)
    {
        wood -= woodCost;
        stone -= stoneCost;
    }
}
}
