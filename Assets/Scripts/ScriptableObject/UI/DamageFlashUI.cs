using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    public CanvasGroup flashGroup;
    public float flashAlpha = 0.3f;
    public float fadeDuration = 0.5f;

    private Coroutine currentFlashCoroutine;

    private void Awake()
    {
        if (flashGroup != null)
            flashGroup.alpha = 0f; // 시작 시 숨김
    }

    public void Flash()
    {
        if (flashGroup == null) return;

        // 이전 깜빡임이 있으면 중단하고 다시 시작
        if (currentFlashCoroutine != null)
            StopCoroutine(currentFlashCoroutine);

        currentFlashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // 알파 즉시 설정 (플래시)
        flashGroup.alpha = flashAlpha;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            flashGroup.alpha = Mathf.Lerp(flashAlpha, 0f, timer / fadeDuration);
            yield return null;
        }

        flashGroup.alpha = 0f;
        currentFlashCoroutine = null;
    }
}
