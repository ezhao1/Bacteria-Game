using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (musicSlider == null || sfxSlider == null)
        {
            return;
        }
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", AudioManager.DEFAULT_VOLUME_VALUE);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", AudioManager.DEFAULT_VOLUME_VALUE);
    }

    public void SetMusicVolume(float v)
    {
        PlayerPrefs.SetFloat("MusicVolume", v);
        AudioManager.Instance.musicSource.volume = v;
    }
    public void SetSFXVolume(float v)
    {
        PlayerPrefs.SetFloat("SFXVolume", v);
        AudioManager.Instance.sfxSource.volume = v;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");

        AudioManager.Instance.PlayClick();
    }
}
