using UnityEngine;
using TMPro;
using System.Collections;

public class NoticeUI : MonoBehaviour
{
    public static NoticeUI Instance;

    [SerializeField] private GameObject noticePanel;
    [SerializeField] private TMP_Text noticeText;
    [SerializeField] private float displayDuration = 1f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Show(string message)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        noticeText.text = message;
        noticePanel.SetActive(true);
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        noticePanel.SetActive(false);
        hideCoroutine = null;
    }
}