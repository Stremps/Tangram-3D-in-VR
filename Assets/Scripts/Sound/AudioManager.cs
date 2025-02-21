using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public static AudioManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playMusic("Theme");
    }

    public void playMusic(string name)
    {
        Sound targetSound = Array.Find(musicSounds, x => x.name == name);

        if(targetSound == null)
        {
            Debug.Log($"[Music Error] - Sound '{name}' not found!");
        }
        else
        {
            musicSource.clip = targetSound.clip;
            musicSource.Play();
        }
    }

    public void playSoundFX(string name)
    {
        Sound targetSound = Array.Find(sfxSounds, x => x.name == name);

        if(targetSound == null)
        {
            Debug.Log($"[Music Error] - Sound '{name}' not found!");
        }
        else
        {
            sfxSource.PlayOneShot(targetSound.clip);
        }
    }
}
