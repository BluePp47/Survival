using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private List<Dialogues> dialogueList;
    [SerializeField] private List<Button> buttons;

    [SerializeField] private TMP_Text content;
    [SerializeField] private Button btnNext;
    [SerializeField] private TMP_Text buttonText;

    private bool IsLastDialog()
    {
        return _currentIndex == dialogueList.Count - 1;
    }

    private int _currentIndex;

    private void Awake()
    {
        btnNext.onClick.AddListener(OnNextButtonPressed);
    }

    private void OnEnable() //동작시시
    {
        buttonText.text = "다음";
        _currentIndex = 0;
        ShowDialog();
    }

    private void ShowDialog()
    {
        Dialogues current = dialogueList[_currentIndex];

        content.text = current.dialogue;

        for (var i = 0; i < buttons.Count; i++)
        {
            Button choiceButton = buttons[i];

            // 선택지 버튼이 눌렸을때 호출될 함수를 초기화
            choiceButton.onClick.RemoveAllListeners();

            // 버튼의 갯수보다 선택지의 갯수가 적으면, 버튼을 비활성화
            if (i >= current.SelectCount())
            {
                choiceButton.gameObject.SetActive(false);
                continue;
            }

            choiceButton.gameObject.SetActive(true);

            choiceButton.onClick.AddListener(current.events[i].Invoke);

            var choiceText = choiceButton.transform.GetComponentInChildren<TMP_Text>();
            choiceText.text = current.Selections[i];
        }
    }

    private void OnNextButtonPressed()
    {
        if (IsLastDialog())
            gameObject.SetActive(false);

        else
        {
            _currentIndex++;
            ShowDialog();

            if (IsLastDialog())
                buttonText.text = "나가기";
        }
    }

}
