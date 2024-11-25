using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject helpScreen;
    [SerializeField] GameObject levelSelectScreen;
    [SerializeField] AudioClip buttonClickSFX;
    private AudioSource sfxPlayer;

    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
        sfxPlayer.volume = VolumeManager.Instance.sfxVolume;
    }

    public void StartGame()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        levelSelectScreen.SetActive(true);
    }

    public void QuitGame()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void HelpScreen()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        Time.timeScale = 0;
        helpScreen.SetActive(true);
    }

    public void LeaveHelpScreen()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        Time.timeScale = 1;
        helpScreen.SetActive(false);
    }

    public void LeaveLevelSelect()
    {
        sfxPlayer.PlayOneShot(buttonClickSFX);
        levelSelectScreen.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        // Load the (first) level with the appropriate entrance location
        sfxPlayer.PlayOneShot(buttonClickSFX);
        VolumeManager.Instance.levelNr = level; // Yes, I know know I should rename VolumeManager if I use it like this, shut up
        SceneManager.LoadScene(1); // Make sure the (first) level has index 1 in the build settings !!!
    }
}
