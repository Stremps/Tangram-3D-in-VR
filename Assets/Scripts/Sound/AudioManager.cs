using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
        Music_Play("Theme");
    }

    public void Music_Play(string name)
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
    
    public void SFX_PlayGlobal(string name)
    {
        Sound targetSound = Array.Find(sfxSounds, x => x.name == name);

        if(targetSound == null)
        {
            Debug.Log($"[SFX Error] - Sound '{name}' not found!");
        }
        else
        {
            sfxSource.PlayOneShot(targetSound.clip);
        }
    }

    public void SFX_PlayAtSource(string name, UnityEngine.Vector3 position)
    {
        // Search the soundFX
        Sound targetSound = Array.Find(sfxSounds, x => x.name == name);

        // If not found, send error message
        if(targetSound == null)
        {
            Debug.Log($"[SFX Error] - Sound '{name}' not found!");
        }
        // If found play the song at the transform of the object
        else
        {

            AudioSource.PlayClipAtPoint(targetSound.clip, position, sfxSource.volume);
        }
    }

    public void SFX_PlayAttached(string name, GameObject gameObject)
    {
        // Search the sound

        // If not found, send erro message

        // If found play the following the object
    }
}
