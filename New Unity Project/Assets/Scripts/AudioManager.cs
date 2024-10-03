using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music Settings")]
    public float musicVolume = .3f;

    [SerializeField]
    AudioClip chaseMusic;
    AudioSource chaseMusicSource;

    [SerializeField]
    AudioClip ambientMusic;
    AudioSource ambientMusicSource;

    [Header("SFX Settings")]
    public float sfxVolume = .4f;

    [SerializeField]
    AudioClip intercomBeep;
    AudioSource intercomBeepSource;

    [SerializeField]
    AudioClip airlockScene;
    AudioSource airlockSceneSource;
    public float airlockSceneLength;

    [SerializeField]
    AudioClip crewQuartersScene;
    AudioSource crewQuartersSource;
    public float crewQuartersSceneLength;

    [SerializeField]
    AudioClip doorCrash;
    AudioSource doorCrashSource;

    [SerializeField]
    AudioClip engineeringScene;
    AudioSource engineeringSceneSource;
    public float engineeringSceneLength;

    [SerializeField]
    AudioClip mainDeckScene;
    AudioSource mainDeckSceneSource;
    public float mainDeckSceneLength;

    [SerializeField]
    AudioClip mainDeckSceneIntro;
    AudioSource mainDeckSceneIntroSource;
    public float mainDeckSceneIntroLength;

    private void Start()
    {
        InitializeMusic();
        InitializeSFX();
        PlayAmbientMusic();
    }

    void InitializeMusic()
    {
        if(chaseMusic != null)
        {
            chaseMusicSource = gameObject.AddComponent<AudioSource>();
            chaseMusicSource.clip = chaseMusic;
            chaseMusicSource.loop = true;
            chaseMusicSource.volume = musicVolume;
        }

        if(ambientMusic != null)
        {
            ambientMusicSource = gameObject.AddComponent<AudioSource>();
            ambientMusicSource.clip = ambientMusic;
            ambientMusicSource.loop = true;
            ambientMusicSource.volume = musicVolume;
        }
    }

    void InitializeSFX()
    {
        if (intercomBeep != null)
        {
            intercomBeepSource = gameObject.AddComponent<AudioSource>();
            intercomBeepSource.clip = intercomBeep;
            intercomBeepSource.loop = true;
            intercomBeepSource.volume = sfxVolume;
        }

        if (airlockScene != null)
        {
            airlockSceneSource = gameObject.AddComponent<AudioSource>();
            airlockSceneSource.clip = airlockScene;
            airlockSceneSource.loop = false;
            airlockSceneSource.volume = sfxVolume;
        }

        if (crewQuartersScene != null)
        {
            crewQuartersSource = gameObject.AddComponent<AudioSource>();
            crewQuartersSource.clip = crewQuartersScene;
            crewQuartersSource.loop = false;
            crewQuartersSource.volume = sfxVolume;
        }

        if (doorCrash != null)
        {
            doorCrashSource = gameObject.AddComponent<AudioSource>();
            doorCrashSource.clip = doorCrash;
            doorCrashSource.loop = true;
            doorCrashSource.volume = sfxVolume;
        }

        if (engineeringScene != null)
        {
            engineeringSceneSource = gameObject.AddComponent<AudioSource>();
            engineeringSceneSource.clip = engineeringScene;
            engineeringSceneSource.loop = false;
            engineeringSceneSource.volume = sfxVolume;
        }

        if (mainDeckScene != null)
        {
            mainDeckSceneSource = gameObject.AddComponent<AudioSource>();
            mainDeckSceneSource.clip = mainDeckScene;
            mainDeckSceneSource.loop = false;
            mainDeckSceneSource.volume = sfxVolume;
        }

        if (mainDeckSceneIntro != null)
        {
            mainDeckSceneIntroSource = gameObject.AddComponent<AudioSource>();
            mainDeckSceneIntroSource.clip = mainDeckSceneIntro;
            mainDeckSceneIntroSource.loop = false;
            mainDeckSceneIntroSource.volume = sfxVolume;
        }
    }

    public void PlayAmbientMusic()
    {
        ambientMusicSource.Play();
    }

    public void StopAmbientMusic()
    {
        ambientMusicSource.Stop();
    }

    public void PlayChaseMusic()
    {
        chaseMusicSource.Play();
    }

    public void StopChaseMusic()
    {
        chaseMusicSource.Stop();
    }

    public void PlayIntercomBeep()
    {
        intercomBeepSource.Play();
    }

    public void StopIntercomBeep()
    {
        intercomBeepSource.Stop();
    }

    public AudioSource GetIntercomBeep()
    {
        return intercomBeepSource;
    }

    public void PlayAirlockScene()
    {
        airlockSceneSource.Play();
    }

    public void StopAirlockScene()
    {
        airlockSceneSource.Stop();
    }

    public void PlayCrewQuartersScene()
    {
        crewQuartersSource.Play();
    }

    public void StopCrewQuartersScene()
    {
        crewQuartersSource.Stop();
    }

    public void PlayDoorCrash()
    {
        doorCrashSource.Play();
    }

    public void StopDoorCrash()
    {
        doorCrashSource.Stop();
    }

    public void PlayEngineeringScene()
    {
        engineeringSceneSource.Play();
    }

    public void StopEngineeringScene()
    {
        engineeringSceneSource.Stop();
    }
    public void PlayMainDeckScene()
    {
        mainDeckSceneSource.Play();
    }

    public void StopMainDeckScene()
    {
        mainDeckSceneSource.Stop();
    }

    public void PlayMainDeckSceneIntro()
    {
        mainDeckSceneIntroSource.Play();
    }

    public void StopMainDeckSceneIntro()
    {
        mainDeckSceneIntroSource.Stop();
    }

}
