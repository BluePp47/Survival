using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/quest_save.json";
    //Application.persistentDataPath는 플랫폼에 따라 안전하게 파일을 저장할 수 있는 경로를 제공합니다.
    //Windows에서는 C:/Users/사용자명/AppData/LocalLow/회사명/프로젝트명/
    //맥에서는 Users/사용자명/Library/Application Support/회사명/프로젝트명/



    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("퀘스트 저장됨: " + path);
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("저장 파일 없음. 기본값 반환");
            return new SaveData(); // 기본값
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void ResetGame()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("저장 데이터 리셋 완료");
        }
        else
        {
            Debug.Log("리셋할 저장 파일이 없음");
        }
    }
}
