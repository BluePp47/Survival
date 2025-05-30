using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    /// <summary>
    /// Additive 모드로 씬을 로드하고 활성화하는 코루틴
    /// </summary>
    public IEnumerator LoadAdditiveSceneAndActivate(string sceneName)
    {
        Debug.Log($"Additive 씬 로드 시작: {sceneName}");

        // 1. 씬이 이미 로드되어 있는지 확인
        Scene existingScene = SceneManager.GetSceneByName(sceneName);
        if (existingScene.isLoaded)
        {
            Debug.Log($"씬이 이미 로드되어 있음: {sceneName}");
            SceneManager.SetActiveScene(existingScene);
            yield break;
        }

        // 2. Additive 모드로 씬 로드 시작
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // 3. 씬 로드 완료까지 대기
        while (loadOperation is { isDone: false })
        {
            Debug.Log($"로딩 진행률: {loadOperation.progress * 100f}%");
            yield return null;
        }

        Debug.Log($"씬 로드 완료: {sceneName}");

        // 4. 한 프레임 대기 (씬이 완전히 초기화될 때까지)
        yield return null;

        // 5. 로드된 씬을 활성 씬으로 설정
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        if (newScene.isLoaded)
        {
            SceneManager.SetActiveScene(newScene);
            Debug.Log($"활성 씬 설정 완료: {sceneName}");

            // 6. 추가 한 프레임 대기 (SetActiveScene이 완전히 적용될 때까지)
            yield return null;

            // 7. 라이팅 환경 강제 재설정
            yield return ForceRefreshLighting();
        }
        else
        {
            Debug.LogError($"씬 로드 실패: {sceneName}");
        }
    }

    /// <summary>
    /// 이전 씬을 언로드하고 새 씬을 Additive로 로드하는 코루틴
    /// </summary>
    public IEnumerator ReplaceSceneAdditive(string oldSceneName, string newSceneName)
    {
        Debug.Log($"씬 교체 시작: {oldSceneName} -> {newSceneName}");

        // 1. 새 씬을 Additive로 로드
        yield return LoadAdditiveSceneAndActivate(newSceneName);

        // 2. 이전 씬 언로드
        Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        if (oldScene.isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(oldScene);
            yield return unloadOperation;
            Debug.Log($"이전 씬 언로드 완료: {oldSceneName}");
        }

        // 3. 메모리 정리
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();

        Debug.Log($"씬 교체 완료: {newSceneName}");
    }

    /// <summary>
    /// 라이팅 환경 강제 재설정
    /// </summary>
    private IEnumerator ForceRefreshLighting()
    {
        Debug.Log("라이팅 환경 재설정 시작");

        // Skybox 재적용
        Material currentSkybox = RenderSettings.skybox;
        RenderSettings.skybox = null;
        yield return null;
        RenderSettings.skybox = currentSkybox;

        // Ambient Light 재설정
        float currentAmbientIntensity = RenderSettings.ambientIntensity;
        RenderSettings.ambientIntensity = 0f;
        yield return null;
        RenderSettings.ambientIntensity = currentAmbientIntensity;

        // 환경 반사 업데이트
        DynamicGI.UpdateEnvironment();

        Debug.Log("라이팅 환경 재설정 완료");
    }
}
