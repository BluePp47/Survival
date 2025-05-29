using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characters;        // 캐릭터 프리팹 배열
    private int currentIndex = 0;          // 현재 선택된 캐릭터 인덱스
    private GameObject currentCharacter;   // 현재 씬에 표시된 캐릭터
    
    public Button leftButton;              
    public Button rightButton;             

    public Button startButton;
    
    void Start()
    {
        ShowCharacter(currentIndex); // 첫 캐릭터 보여주기

        leftButton.onClick.AddListener(ShowPreviousCharacter);
        rightButton.onClick.AddListener(ShowNextCharacter);
        startButton.onClick.AddListener(StartGame);
    }

    void ShowCharacter(int index)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(characters[index], Vector3.zero, Quaternion.identity);
        currentCharacter.transform.position = new Vector3(0, 0, 0); // 중앙에 위치
    }

    void ShowPreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
        ShowCharacter(currentIndex);
    }

    void ShowNextCharacter()
    {
        currentIndex = (currentIndex + 1) % characters.Length;
        ShowCharacter(currentIndex);
    }
    
    void StartGame()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        SceneManager.LoadScene("GameScene"); // 이동할 씬 이름
    }
    
    public float rotationSpeed = 50f; // 회전 속도 (값이 클수록 빠름)

    void Update()
    {
        if (currentCharacter != null)
        {
            currentCharacter.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
