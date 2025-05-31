using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [System.Serializable]
    public class NamedUI
    {
        public string name;
        public GameObject uiObject;
    }

    public List<NamedUI> uiList;

    private void Awake()
    {
        // 씬마다 UIController 하나만 존재하게 함
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 씬 전환 시 함께 파괴되도록 DontDestroyOnLoad 사용하지 않음
    }

    // 특정 UI 켜기 (하나만 켜짐)
    public void OpenUI(string uiName)
    {
        foreach (var ui in uiList)
        {
            ui.uiObject.SetActive(ui.name == uiName);
        }
    }

    // 모든 UI 끄기
    public void CloseAllUI()
    {
        Debug.Log("모든UI를닫았습니다");
        foreach (var ui in uiList)
        {
            ui.uiObject.SetActive(false);
        }
    }
}