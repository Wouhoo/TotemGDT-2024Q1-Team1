using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    // This game object is used to store (music/sfx volume) values between scene loads
    // and also abused for storing the entrance number and time to beat level
    public static VolumeManager Instance;

    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public int levelNr = 0;      // Level/entrance number the player enters the level in
    public int exitNr = 0;       // Number of the exit where the player finished the level
    public float levelTime = 0f; // Time the player took to beat the level

    // Set up the main manager properly
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
