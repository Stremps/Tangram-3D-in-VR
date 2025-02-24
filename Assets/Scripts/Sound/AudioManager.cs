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

    public void Music_PlayAttached(string name, Transform parentTransform, float pitch = 1.0f)
    {
        // Search the sound
        Sound targetSound = Array.Find(musicSounds, x => x.name == name);

        // If not found, send erro message
        if( targetSound == null)
        {
            Debug.Log($"[Music Error] - Sound '{name}' not found!");
        }
        // If found play the following the object
        else
        {
            //Creates the game object
            GameObject childSoundObject = new GameObject("TempMusic");

            // Attach as a Child
            childSoundObject.transform.SetParent(parentTransform);
            childSoundObject.transform.localPosition = UnityEngine.Vector3.zero;

            // Add AudioSource in the temp child game object
            AudioSource audioSource = childSoundObject.AddComponent<AudioSource>();
            audioSource.clip = targetSound.clip;
            audioSource.pitch = pitch;
            audioSource.volume = musicSource.volume;
            audioSource.Play();

            // After the clip lenght, destroy de object
            Destroy(childSoundObject, targetSound.clip.length/pitch);
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

    public void SFX_PlayAtSource(string name, UnityEngine.Vector3 position, float pitch = 1.0f)
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
            // Creates the game object
            GameObject soundObject = new GameObject("TempSoundFX");
            soundObject.transform.position = position;
        
            // Add AudioSource in the temp game object
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = targetSound.clip;
            audioSource.pitch = pitch;
            audioSource.volume = sfxSource.volume;
            audioSource.Play();

            // After the clip lenght, destroy the object
            Destroy(soundObject, targetSound.clip.length/pitch);
        }
    }

    public void SFX_PlayAttached(string name, Transform parentTransform, float pitch = 1.0f)
    {
        // Search the sound
        Sound targetSound = Array.Find(sfxSounds, x => x.name == name);

        // If not found, send erro message
        if( targetSound == null)
        {
            Debug.Log($"[SFX Erro] - Sound '{name}' not found!");
        }
        // If found play the following the object
        else
        {
            //Creates the game object
            GameObject childSoundObject = new GameObject("TempSoundFX");

            // Attach as a Child
            childSoundObject.transform.SetParent(parentTransform);
            childSoundObject.transform.localPosition = UnityEngine.Vector3.zero;

            // Add AudioSource in the temp child game object
            AudioSource audioSource = childSoundObject.AddComponent<AudioSource>();
            audioSource.clip = targetSound.clip;
            audioSource.pitch = pitch;
            audioSource.volume = sfxSource.volume;
            audioSource.Play();

            // After the clip lenght, destroy de object
            Destroy(childSoundObject, targetSound.clip.length/pitch);
        }
        
    }
}
