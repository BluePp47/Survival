using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NoticeUI : MonoBehaviour
{
    public static NoticeUI Instance;

    [SerializeField] private GameObject noticePanel;
    [SerializeField] private TMP_Text noticeText;
    
    private Queue<NoticeMessage> messageQueue = new Queue<NoticeMessage>();
    private Coroutine displayCoroutine;    
    // private Coroutine hideCoroutine; 

    // 싱글톤
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // // 알림창을 띄우고 displayDuration 시간 뒤에 사라지게 함.
    // public void Show(string message)
    // {
    //     if (hideCoroutine != null)
    //     {
    //         StopCoroutine(hideCoroutine);
    //     }

    //     noticeText.text = message;
    //     noticePanel.SetActive(true);
    //     hideCoroutine = StartCoroutine(HideAfterDelay());
    // }

    // private IEnumerator HideAfterDelay()
    // {
    //     yield return new WaitForSeconds(displayDuration);
    //     noticePanel.SetActive(false);
    //     hideCoroutine = null;
    // }

//      public void Show(string message, float time)
//     {
//         messageQueue.Enqueue(message);

//         if (displayCoroutine == null)
//         {
//             displayCoroutine = StartCoroutine(DisplayQueue(time));
//         }
//     }

//     private IEnumerator DisplayQueue(float time)
//     {
//         while (messageQueue.Count > 0)
//         {
//             string currentMessage = messageQueue.Dequeue();

//             noticeText.text = currentMessage;
//             noticePanel.SetActive(true);

//             yield return new WaitForSeconds(time);

//             noticePanel.SetActive(false);
//             yield return new WaitForSeconds(0.2f); // 살짝 딜레이 주기 (선택사항)
//         }

//         displayCoroutine = null;
//     }


// //   public class NoticeUI : MonoBehaviour
// // {
// //     public static NoticeUI Instance;

// //     [SerializeField] private GameObject noticePanel;
// //     [SerializeField] private TMP_Text noticeText;
// //     [SerializeField] private float displayDuration = 1f;

    
// //     private Coroutine displayCoroutine;

// //     // 싱글톤
// //     private void Awake()
// //     {
// //         if (Instance == null) Instance = this;
// //         else Destroy(gameObject);
// //     }


private struct NoticeMessage // 🔧
    {
        public string message;     // 🔧
        public float duration;     // 🔧

        public NoticeMessage(string message, float duration) // 🔧
        {
            this.message = message;
            this.duration = duration;
        }
    }

    // 🔧 메시지와 시간 둘 다 받음
    public void Show(string message, float time) // 🔧
    {
        messageQueue.Enqueue(new NoticeMessage(message, time)); // 🔧

        if (displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplayQueue()); // 🔧 (파라미터 제거)
        }
    }


    // 🔧 메시지별로 저장된 시간 사용
    private IEnumerator DisplayQueue() // 🔧
    {
        while (messageQueue.Count > 0)
        {
            NoticeMessage msg = messageQueue.Dequeue(); // 🔧

            noticeText.text = msg.message; // 🔧
            noticePanel.SetActive(true);

            yield return new WaitForSeconds(msg.duration); // 🔧

            noticePanel.SetActive(false);
            yield return new WaitForSeconds(0.2f); // 살짝 딜레이 (선택 사항)
        }

        displayCoroutine = null;
    }
}
   