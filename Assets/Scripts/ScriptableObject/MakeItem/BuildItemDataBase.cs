using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Building/Build Item Database")]
public class BuildItemDatabase : ScriptableObject
{
    public List<BuildItem> buildItems;
}