using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door : MonoBehaviour
{

    public Inventory inventory;
    public GameObject loadingUI; // Inspector에서 할당

    public void LoadSceneWithPause_Enter()
    {
        inventory = FindObjectOfType<Inventory>();
        SaveData data = inventory.GetSaveData();
        SaveSystem.SaveGame(data);
        foreach (var item in data.inventoryItems)
        {
            Debug.Log($"저장되는 아이템: {item.itemID}, 수량: {item.quantity}");
        }
        StartCoroutine(EnterBuilding());
    }

    public void LoadSceneWithPause_Exit()
    {
        inventory = FindObjectOfType<Inventory>();
        SaveData data = inventory.GetSaveData();
        SaveSystem.SaveGame(data);
        foreach (var item in data.inventoryItems)
        {
            Debug.Log($"저장되는 아이템: {item.itemID}, 수량: {item.quantity}");
        }

        StartCoroutine(ExitBuilding());
    }


    IEnumerator EnterBuilding()
    {   
 // SaveSystem은 당신의 저장 로직 클래스

        // 모든 게임 정지
        Time.timeScale = 0f;

        // 로딩 UI 켜기
        loadingUI.SetActive(true);

        // 살짝 대기 (로딩UI가 화면에 표시될 수 있도록)
        yield return new WaitForSecondsRealtime(1.5f);

        // 비동기 로딩 시작
        AsyncOperation async = SceneManager.LoadSceneAsync("InBuilding");

        // 씬이 로드 완료될 때까지 대기
        while (!async.isDone)
        {
            yield return null;
        }
    }

        IEnumerator ExitBuilding()
    {
        SaveData data = inventory.GetSaveData();
        SaveSystem.SaveGame(data); // SaveSystem은 당신의 저장 로직 클래스

        // 모든 게임 정지
        Time.timeScale = 0f;

        // 로딩 UI 켜기
        loadingUI.SetActive(true);

        // 살짝 대기 (로딩UI가 화면에 표시될 수 있도록)
        yield return new WaitForSecondsRealtime(1.5f);

        // 비동기 로딩 시작
        AsyncOperation async = SceneManager.LoadSceneAsync("3Dsurvibe");

        // 씬이 로드 완료될 때까지 대기
        while (!async.isDone)
        {
            yield return null;
        }
    }

    
    // Player가 문에 충돌시 LoadSceneWithPause() 실행
    void Interact()
    {
        if (SceneManager.GetActiveScene().name == "3Dsurvibe") // 현재 씬이 3Dsurvibe일 때
        {        
            LoadSceneWithPause_Enter();
        }
        else if(SceneManager.GetActiveScene().name == "InBuilding") // 현재 씬이 InBuilding일 때
        {        
            LoadSceneWithPause_Exit();
        }
    }
}
    