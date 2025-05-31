using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Audio Clips")]    
    public AudioClip bgm; // 배경음악(바람소리)
    public AudioClip uiClickSound; // 클릭할때
    public AudioClip openDoorSound; // 문 열때 


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBGM(bgm);
    }

    void OnEnable()
    {   
        Debug.Log("sound onenable");
        SoundEvents.OnUIClick += PlayUIClick;
        SoundEvents.OnPlaySFX += PlaySFX;
        SoundEvents.OnPlaySFX2 += PlaySFX;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("sound ondisable");
        SoundEvents.OnUIClick -= PlayUIClick;
        SoundEvents.OnPlaySFX -= PlaySFX;
        SoundEvents.OnPlaySFX2 -= PlaySFX;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // bgm 재생
     public void PlayBGM(AudioClip clip, bool loop = true) 
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
    }

    // bgm 중지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // 클릭시 소리 재생
    public void PlayUIClick()
    {
        if (uiClickSound != null)
            sfxSource.PlayOneShot(uiClickSound);
    }

    // 외부 sound 가져와서 재생
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // 외부 sound(배열) 가져와서 재생
    public void PlaySFX(AudioClip[] clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip[Random.Range(0,clip.Length)]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();

        foreach (var listener in listeners)
        {
            if (listener.gameObject.scene != scene) // 현재 씬이 아닌 경우 삭제
            {
                Destroy(listener.gameObject);
            }
        }
    }
}
