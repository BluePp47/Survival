using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingUI; // Inspector에서 할당

    public void LoadSceneWithPause()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    IEnumerator LoadSceneRoutine()
    {
        // 모든 게임 정지
        Time.timeScale = 0f;

        // 로딩 UI 켜기
        loadingUI.SetActive(true);

        // 살짝 대기 (로딩UI가 화면에 표시될 수 있도록)
        yield return new WaitForSecondsRealtime(2.0f);

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

    void Update()
    {
        transform.Rotate(Vector3.forward, 180f * Time.unscaledDeltaTime); // 로딩 스피너 회전
    }
}