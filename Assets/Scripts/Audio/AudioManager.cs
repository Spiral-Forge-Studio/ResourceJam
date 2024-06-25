using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("References")]
    [SerializeField] private AudioSource SAMFireAS;
    [SerializeField] private AudioSource ExplosionAS;
    [SerializeField] private AudioSource TeslaAS;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;

    private Dictionary<string, AudioSource> audioDict;
    private Dictionary<string, AudioClip[]> sfxDict;

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("SFX")]
    [SerializeField] private AudioClip[] SAMFireSFX;
    [SerializeField] private AudioClip[] ExplosionSFX;
    [SerializeField] private AudioClip[] TeslaSFX;

    [Header("ButtonSFX")]
    [SerializeField] private AudioClip[] buttonSfxList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioDict();
            InitializeSFXDict();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic(0);
    }

    private void InitializeAudioDict()
    {
        audioDict = new Dictionary<string, AudioSource>
        {
            { "SAMFire", SAMFireAS },
            { "Explosion", ExplosionAS },
            { "Tesla", TeslaAS },
            { "General", audioSource }
        };
    }

    private void InitializeSFXDict()
    {
        sfxDict = new Dictionary<string, AudioClip[]>
        {
            { "SAMFire", SAMFireSFX },
            { "Explosion", ExplosionSFX },
            { "Tesla", TeslaSFX }
        };
    }

    public void PlayMusic(int musicNumber, float volume = 1)
    {
        musicSource.clip = musicList[musicNumber];
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlaySFX(string audioKey, float volume = 1)
    {
        if (audioDict.TryGetValue(audioKey, out AudioSource source) && sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }

            int randomIndex = Random.Range(0, clips.Length);
            source.clip = clips[randomIndex];
            source.volume = volume;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlayButtonSFX(int buttonNumber, float volume = 1)
    {
        audioSource.clip = buttonSfxList[buttonNumber];
        audioSource.volume = volume;
        audioSource.Play();
    }
}
