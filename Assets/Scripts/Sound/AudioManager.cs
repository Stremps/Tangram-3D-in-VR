using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] ambienceSounds, musicSounds, sfxSounds;
    public AudioSource ambienceSource, musicSource, sfxSource;
    public static AudioManager Instance;
    public bool musicOnStart;
    public string startMusicName;
    private string actualScene;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            actualScene = SceneManager.GetActiveScene().name;
            DontDestroyOnLoad(gameObject);
        }
        else if(actualScene != SceneManager.GetActiveScene().name) // if the scene change
        {
            actualScene = SceneManager.GetActiveScene().name; // Change the actualScene name
            
            // Refresh the sounds collection
            Instance.musicSounds = musicSounds;
            Instance.sfxSounds = sfxSounds;
            Instance.musicOnStart = musicOnStart;

            // Destroy the gameObject
            Destroy(gameObject);

            // Start again, based in the new scene
            Instance.Start();
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Audio Manager foi de vasco");
        }
    }

    private void Start()
    {
        if(musicOnStart == true)
        {   
            Music_Play(startMusicName);
        }
        
    }

        public void Ambience_PlayAttached(string name, Transform parentTransform)
    {
        // Search the sound
        Sound targetSound = Array.Find(ambienceSounds, x => x.name == name);

        // If not found, send error message
        if( targetSound == null)
        {
            Debug.Log($"[Ambience Erro] - Sound '{name}' not found!");
        }
        // If found play the following the object
        else
        {
            //Creates the game object
            GameObject childSoundObject = new GameObject("TempAmbienceSound");

            // Attach as a Child
            childSoundObject.transform.SetParent(parentTransform);
            childSoundObject.transform.localPosition = UnityEngine.Vector3.zero;

            // Add AudioSource in the temp child game object
            AudioSource audioSource = childSoundObject.AddComponent<AudioSource>();
            audioSource.clip = targetSound.clip;
            audioSource.spatialBlend = 1.0f;
            audioSource.volume = ambienceSource.volume;
            audioSource.loop = true;
            audioSource.Play();
        }
        
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

    public void Music_FadeIn(float duration)
    {
        Music_Fade(duration, 0, 0.2f);
    }

    public void Music_FadeOut(float duration)
    {
        Music_Fade(duration, 0.2f, 0);
    }

    public void Music_Fade(float duration, float volumeIn, float volumeOut)
    {
        StartCoroutine(Music_FadeRoutine(duration, volumeIn, volumeOut));
    }

    private IEnumerator Music_FadeRoutine(float duration, float volumeIn, float volumeOut)
    {
        float timer = 0f;

        while(timer <= duration)
        {
            musicSource.volume = Mathf.Lerp(volumeIn, volumeOut, timer / duration);
            timer+= Time.deltaTime;
            yield return null;
        }

        musicSource.volume = volumeOut;
        

        yield break;
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
        SFX_PlayAtSource(name, position, 1f, 1f);
    }

    public void SFX_PlayAtSource(string name, UnityEngine.Vector3 position, float pitch)
    {
        SFX_PlayAtSource(name, position, pitch, 1f);
    }

    public void SFX_PlayAtSource(string name, UnityEngine.Vector3 position, float pitch, float volume)
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
            audioSource.volume = sfxSource.volume * volume;
            audioSource.spatialBlend = 1.0f;
            audioSource.Play();

            // After the clip lenght, destroy the object
            Destroy(soundObject, targetSound.clip.length/pitch);
        }
    }

    public void SFX_PlayAttached(string name, Transform parentTransform, float pitch = 1.0f)
    {
        // Search the sound
        Sound targetSound = Array.Find(sfxSounds, x => x.name == name);

        // If not found, send error message
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
            audioSource.spatialBlend = 1.0f;
            audioSource.volume = sfxSource.volume;
            audioSource.Play();

            // After the clip lenght, destroy de object
            Destroy(childSoundObject, targetSound.clip.length/pitch);
        }
        
    }
}
