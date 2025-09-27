using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;

    private void Awake()
    {
        soundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "SOUND: " + SoundManager.Instance.GetSoundVolume();
        });

        musicVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ToggleMusic();
            musicVolumeTextMesh.text = MusicManager.Instance.IsMusicEnabled() ? "MUSIC: ON" : "MUSIC: OFF";
        });

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnpauseGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        soundVolumeTextMesh.text = "SOUND: " + SoundManager.Instance.GetSoundVolume();
        musicVolumeTextMesh.text = MusicManager.Instance.IsMusicEnabled() ? "MUSIC: ON" : "MUSIC: OFF";

        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
