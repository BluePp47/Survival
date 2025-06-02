using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    private Dictionary<string, ItemData> itemDict;

    public void Init()
    {
        itemDict = new Dictionary<string, ItemData>();

        foreach (var item in items)
        {
            if (!itemDict.ContainsKey(item.displayName))
            {
                itemDict.Add(item.displayName, item);
            }
        }
    }

    public ItemData GetItemById(string id)
    {
        if (itemDict == null)
            Init();

        if (itemDict.TryGetValue(id, out var item))
        {
            return item;
        }

        Debug.LogWarning($"아이템 ID '{id}'를 찾을 수 없습니다.");
        return null;
    }
    
}
public static class ItemDatabaseManager
{
    public static ItemDatabase Instance;

    [RuntimeInitializeOnLoadMethod]
    private static void LoadDatabase()
    {
        Instance = Resources.Load<ItemDatabase>("ItemDatabase");
        if (Instance != null)
            Instance.Init();
        else
            Debug.LogError("ItemDatabase를 Resources 폴더에서 찾을 수 없습니다.");
    }
}