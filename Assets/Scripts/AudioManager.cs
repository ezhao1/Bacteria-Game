using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    public static AudioManager Instance => instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(AudioClip c)
    {
        sfxSource.PlayOneShot(c);
    }

    public static float DEFAULT_VOLUME_VALUE = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", DEFAULT_VOLUME_VALUE);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", DEFAULT_VOLUME_VALUE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
