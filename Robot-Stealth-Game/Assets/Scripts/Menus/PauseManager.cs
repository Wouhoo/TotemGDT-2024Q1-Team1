using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject helpScreen;
    [SerializeField] AudioClip buttonClickSFX;
    private AudioSource sfxPlayer;
    private bool paused;

    private void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
        sfxPlayer.volume = VolumeManager.Instance.sfxVolume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!paused)
                Pause();
            else
                Unpause();
        }
    }

    public void Pause()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        pauseScreen.SetActive(true);
        paused = true;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        helpScreen.SetActive(false);
        pauseScreen.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        SceneManager.LoadScene(0);
    }

    public void HelpScreen()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        helpScreen.SetActive(true);
    }

    public void LeaveHelpScreen()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        helpScreen.SetActive(false);
    }
}
