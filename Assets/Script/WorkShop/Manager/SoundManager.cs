using UnityEngine;
using System.Collections.Generic;

public sealed class SoundManager : MonoBehaviour
{
    // 1. Singleton Instance
    private static SoundManager _instance;

    // 2. Public Static Property (Global Access Point)
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SoundManager instance is null! Is it in the scene?");
            }
            return _instance;
        }
    }

    [Header("Audio Sources")]

    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Default Audio Clips")]
    public AudioClip defaultButtonClick;
    public AudioClip defaultBackgroundMusic;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

          
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();


            musicSource.loop = true; 

            PlayMusic(defaultBackgroundMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ------------------- Music Controls -------------------

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // ------------------- SFX Controls -------------------

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // ------------------- Volume Controls -------------------

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }
}