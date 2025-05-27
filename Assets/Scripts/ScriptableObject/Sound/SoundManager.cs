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

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;
    public AudioClip uiClickSound;
    public AudioClip openDoorSound;

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

    void OnEnable()
    {
        SoundEvents.OnUIClick += PlayUIClick;
        SoundEvents.OnPlaySFX += PlaySFX;
    }

    void OnDisable()
    {
        SoundEvents.OnUIClick -= PlayUIClick;
        SoundEvents.OnPlaySFX -= PlaySFX;
    }

    public void PlayUIClick()
    {
        if (uiClickSound != null)
            sfxSource.PlayOneShot(uiClickSound);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX2(AudioClip[] clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip[Random.Range(0,clip.Length)]);
    }
}
