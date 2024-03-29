﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    public float volume;

    private Settings settings;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        settings = FindObjectOfType<Settings>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Update()
    {
        //Update settings script on scene change
        volume = settings.volume;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            s.source.volume = volume;

            s.source.Play();
        }
        else
            Debug.LogWarning("Sound " + name + " was not found.");
    }

    //Update settings instance when switching scenes
    private void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;

    private void OnDisable() => SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        settings = FindObjectOfType<Settings>();
    }
}