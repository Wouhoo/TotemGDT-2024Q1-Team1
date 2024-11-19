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
    [SerializeField] AudioClip buttonClickSFX;
    private AudioSource sfxPlayer;

    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        // Load the first level. MAKE SURE THIS HAS BUILD INDEX 1 IN THE BUILD SETTINGS.
        sfxPlayer.PlayOneShot(buttonClickSFX);
        SceneManager.LoadScene(1);
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
}
