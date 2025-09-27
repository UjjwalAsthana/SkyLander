using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SOUND_VOLUME_MAX = 11;
    public static SoundManager Instance { get; private set; }
    private static int soundVolume = 6;
    public event EventHandler OnSoundVolumeChanged;
    [SerializeField] private AudioClip fuelPickupAudio;
    [SerializeField] private AudioClip coinPickupAudio;
    [SerializeField] private AudioClip crashAudio;
    [SerializeField] private AudioClip successLandingAudio;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                AudioSource.PlayClipAtPoint(successLandingAudio, Camera.main.transform.position,GetSoundNormalized());
                break;
            default:
                AudioSource.PlayClipAtPoint(crashAudio, Camera.main.transform.position, GetSoundNormalized());
                break;
        }
    }

    private void Lander_OnFuelPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupAudio, Camera.main.transform.position, GetSoundNormalized());
    }

    private void Lander_OnCoinPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudio, Camera.main.transform.position, GetSoundNormalized());
    }
    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetSoundVolume()
    {
        return soundVolume;
    }
    public float GetSoundNormalized()
    {
        return ((float)soundVolume) / SOUND_VOLUME_MAX;
    }
}
