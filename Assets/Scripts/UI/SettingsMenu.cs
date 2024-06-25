using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Toggle toggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = masterVolume.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume)*20);
    }
    public void SetMusicVolume()
    {
        float volume = musicVolume.value;
        //AudioManager.instance.MusicVolume(volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume()
    {
        float volume = sfxVolume.value;
        //AudioManager.instance.SFXVolume(volume);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }


    private void LoadVolume()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");


        SetMasterVolume();
    }
}
