using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    private void Start()
    {
        // 버튼 연결
        if (startButton != null) startButton.onClick.AddListener(StartGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("3Dsurvibe"); // 실제 게임 씬 이름
    }

    private void QuitGame()
    {
        Debug.Log("게임 종료 시도");
        Application.Quit();

    }
}