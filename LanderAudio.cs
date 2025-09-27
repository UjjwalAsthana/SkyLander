using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
    }
    private void Start()
    {
        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
    }

    private void SoundManager_OnSoundVolumeChanged(object sender, EventArgs e)
    {
        SoundManager.Instance.GetSoundNormalized();
    }
}
