using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{   
    [Header("캐릭터 선택")]
    public GameObject[] characters;        // 캐릭터 프리팹 배열
    private int currentIndex = 0;          // 현재 선택된 캐릭터 인덱스
    private GameObject currentCharacter;   // 현재 씬에 표시된 캐릭터
    
    [Header("캐릭터 이름")]
    public string[] characterNames; // 캐릭터 이름 배열
    public TextMeshProUGUI nameText; // 이름을 출력할 TMP 텍스트
    
    [Header("버튼")]
    public Button leftButton;              
    public Button rightButton;             

    public Button startButton;

    [SerializeField] AudioClip selectcharacterSound;
    
    private AdditiveSceneLoader sceneLoader;
    void Start()
    {
       
        ShowCharacter(currentIndex); // 첫 캐릭터 보여주기

        leftButton.onClick.AddListener(ShowPreviousCharacter);
        rightButton.onClick.AddListener(ShowNextCharacter);
        startButton.onClick.AddListener(StartGame);
        sceneLoader = GetComponent<AdditiveSceneLoader>();
    }

    
// 씬 교체 (이전 씬 언로드 + 새 씬 로드)
    private void ReplaceWithMenuScene()
    {
        StartCoroutine(sceneLoader.ReplaceSceneAdditive("CharacterSelect", "3Dsurvibe")); //앞에 스타트씬 추가
    }
    
    void ShowCharacter(int index)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(characters[index], Vector3.zero, Quaternion.Euler(0, 140, 0));
        currentCharacter.transform.position = new Vector3(0, 0, 0); // 중앙에 위치
        
        if (nameText != null && characterNames.Length > index)  // 캐릭터 이름 출력
            nameText.text = characterNames[index];
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
    
    void StartGame() //다른씬으로 이동하는 코드 !
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        // SceneManager.LoadScene("3Dsurvibe"); // 이동할 씬 이름
        ReplaceWithMenuScene(); // 이동할 씬 이름
        SoundManager.Instance.PlaySFX(selectcharacterSound);
        Debug.Log("게임씬 이동");
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
