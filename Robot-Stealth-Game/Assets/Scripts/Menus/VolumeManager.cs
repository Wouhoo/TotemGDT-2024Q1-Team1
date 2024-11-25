using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    // This game object is used to store (music/sfx volume) values between scene loads
    // and also abused for storing the entrance number
    public static VolumeManager Instance;

    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public int levelNr = 0;

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
