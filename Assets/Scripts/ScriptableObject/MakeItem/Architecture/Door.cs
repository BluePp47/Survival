using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingUI; // Inspector에서 할당

    public void LoadSceneWithPause_Enter()
    {
        StartCoroutine(EnterBuilding());
    }

    public void LoadSceneWithPause_Exit()
    {
        StartCoroutine(ExitBuilding());
    }

    public 

    IEnumerator EnterBuilding()
    {
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

        // 씬 로드 완료 후 원래 속도 복구 (필요한 경우)
        Time.timeScale = 1f;
    }

        IEnumerator ExitBuilding()
    {
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

        // 씬 로드 완료 후 원래 속도 복구 (필요한 경우)
        Time.timeScale = 1f;
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
    