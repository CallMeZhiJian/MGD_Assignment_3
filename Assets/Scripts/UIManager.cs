using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite SoundOn;
    public Sprite SoundMute;
    [HideInInspector]
    public GameObject SoundButton;
    [HideInInspector]
    public GameObject PauseButton;
    [HideInInspector]
    public GameObject PauseScreen;
    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public GameObject ResultScreen;
    [HideInInspector]
    public GameObject RewardScreen;

    private void Start()
    {
        SoundButton = GameObject.Find("SoundButton");

        AudioManager.instance.PlayBGM();
    }

    private void Update()
    {
        if (AudioManager.instance._BGMSource.mute || AudioManager.instance._SFXSource.mute)
        {
            SoundButton.GetComponent<Image>().sprite = SoundMute; 
        }
        else
        {
            SoundButton.GetComponent<Image>().sprite = SoundOn;
        }
    }

    public void onStart()
    {
        AudioManager.instance.PlaySFX("PressButton");
        SceneManager.LoadScene("GameScene");
    }

    public void onExit()
    {
        AudioManager.instance.PlaySFX("PressButton");
        Application.Quit();
    }

    public void onPauseUnpause()
    {
        AudioManager.instance.PlaySFX("PressButton");

        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;

            PauseScreen.SetActive(false);
            PauseButton.SetActive(true);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;

            PauseScreen.SetActive(true);
            PauseButton.SetActive(false);
        }
    }

    public void PlayAgain()
    {
        AudioManager.instance.PlaySFX("PressButton");
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySFX("PressButton");
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleMuteUnmute()
    {
        AudioManager.instance.PlaySFX("PressButton");
        if (AudioManager.instance._BGMSource.mute || AudioManager.instance._SFXSource.mute)
        {
            AudioManager.instance._BGMSource.mute = false;
            AudioManager.instance._SFXSource.mute = false;
        }
        else
        {
            AudioManager.instance._BGMSource.mute = true;
            AudioManager.instance._SFXSource.mute = true;
        }
    }
}
