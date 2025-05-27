<<<<<<< HEAD
=======
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public enum SFXType
// {
//     Jump,
//     Slide,
//     Coin,
//     GameOver,
//     Hit
//     // 필요한 효과음 타입을 여기 추가!
// }

// [RequireComponent(typeof(AudioSource))]
// public class SoundManager : MonoBehaviour
// {
//     public static SoundManager Instance;

//     [Header("── BGM 세팅 ──")]
//     public AudioClip bgmClip;
//     public AudioSource bgmSource;

//     [Header("── SFX 세팅 ──")]
//     public AudioSource sfxSource;
//     public List<SFXClip> sfxClips;

//     [Header("── 볼륨 조절 슬라이더 ──")]
//     public Slider bgmSlider;
//     public Slider sfxSlider;

//     private Dictionary<SFXType, AudioClip> sfxMap;

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             InitSFXMap();
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//     }

//     void Start()
//     {
//         if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(SetBGMVolume);
//         if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);

//         bgmSource.volume = bgmSlider?.value ?? 1f;
//         sfxSource.volume = sfxSlider?.value ?? 1f;

//         if (bgmClip != null) PlayBGM(bgmClip);
//     }

//     private void InitSFXMap()
//     {
//         sfxMap = new Dictionary<SFXType, AudioClip>();
//         foreach (var entry in sfxClips)
//         {
//             if (!sfxMap.ContainsKey(entry.type) && entry.clip != null)
//                 sfxMap.Add(entry.type, entry.clip);
//         }
//     }

//     public void PlayBGM(AudioClip clip, bool loop = true)
//     {
//         StopAllCoroutines();
//         StartCoroutine(FadeInBGM(clip, loop, 0.5f));
//     }

//     public void PlaySFX(SFXType type)
//     {
//         if (sfxMap != null && sfxMap.TryGetValue(type, out AudioClip clip))
//         {
//             sfxSource.PlayOneShot(clip);
//         }
//         else
//         {
//             Debug.LogWarning($"[SoundManager] SFXType {type}에 매핑된 클립이 없습니다!");
//         }
//     }

//     public void SetBGMVolume(float vol)
//     {
//         bgmSource.volume = vol;
//     }

//     public void SetSFXVolume(float vol)
//     {
//         sfxSource.volume = vol;
//     }

//     private IEnumerator FadeInBGM(AudioClip clip, bool loop, float duration)
//     {
//         bgmSource.clip = clip;
//         bgmSource.loop = loop;
//         float timer = 0f;
//         bgmSource.volume = 0f;
//         bgmSource.Play();
//         float targetVol = bgmSlider?.value ?? 1f;
//         while (timer < duration)
//         {
//             timer += Time.deltaTime;
//             bgmSource.volume = Mathf.Lerp(0f, targetVol, timer / duration);
//             yield return null;
//         }
//         bgmSource.volume = targetVol;
//     }

//     [System.Serializable]
//     public class SFXClip
//     {
//         public SFXType type;
//         public AudioClip clip;
//     }
// }

>>>>>>> 22004982856045d28e6b925938831d48acf7b099
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

<<<<<<< HEAD
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Audio Clips")]    
    public AudioClip bgm; // 배경음악(바람소리)
    public AudioClip uiClickSound; // 클릭할때
    public AudioClip openDoorSound; // 문 열때 

=======
    public AudioSource sfxSource;
    public AudioClip uiClickSound;
    public AudioClip openDoorSound;
>>>>>>> 22004982856045d28e6b925938831d48acf7b099

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

<<<<<<< HEAD
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
=======
    void OnEnable()
    {
        SoundEvents.OnUIClick += PlayUIClick;
        SoundEvents.OnPlaySFX += PlaySFX;
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    }

    void OnDisable()
    {
<<<<<<< HEAD
        Debug.Log("sound ondisable");
        SoundEvents.OnUIClick -= PlayUIClick;
        SoundEvents.OnPlaySFX -= PlaySFX;
        SoundEvents.OnPlaySFX2 -= PlaySFX;
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
=======
        SoundEvents.OnUIClick -= PlayUIClick;
        SoundEvents.OnPlaySFX -= PlaySFX;
    }

>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    public void PlayUIClick()
    {
        if (uiClickSound != null)
            sfxSource.PlayOneShot(uiClickSound);
    }

<<<<<<< HEAD
    // 외부 sound 가져와서 재생
=======
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

<<<<<<< HEAD
    // 외부 sound(배열) 가져와서 재생
    public void PlaySFX(AudioClip[] clip)
=======
    public void PlaySFX2(AudioClip[] clip)
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip[Random.Range(0,clip.Length)]);
    }
}
