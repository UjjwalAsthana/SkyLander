using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource musicAudioSource;
    private bool isMusicEnabled = true; // Always ON by default

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Always start music ON when the game opens
        isMusicEnabled = true;
        UpdateMusicState();
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;
        UpdateMusicState();
    }

    private void UpdateMusicState()
    {
        if (isMusicEnabled)
        {
            musicAudioSource.volume = 1f;
            if (!musicAudioSource.isPlaying)
                musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.volume = 0f;
        }
    }

    public bool IsMusicEnabled()
    {
        return isMusicEnabled;
    }
}
