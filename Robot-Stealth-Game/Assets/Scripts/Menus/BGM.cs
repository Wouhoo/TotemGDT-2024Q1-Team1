using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource audioPlayer;
    [SerializeField] AudioClip hiddenTheme;
    [SerializeField] AudioClip chaseTheme;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        PlayHiddenTheme();
    }

    // Play the "calm" music for when hidden
    public void PlayHiddenTheme()
    {
        audioPlayer.clip = hiddenTheme;
        audioPlayer.Play();
    }

    // Play the chase theme for when spotted
    public void PlayChaseTheme()
    {
        audioPlayer.clip = chaseTheme;
        audioPlayer.Play();
    }

    // DEBUG - Switch between themes by pressing backslash
    // In the actual theme, the chase theme should start playing when *any* enemy spots the player,
    // and the hidden theme should come back when *all* enemies have lost sight of the player.
    bool playChase = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (playChase)
                PlayChaseTheme();
            else
                PlayHiddenTheme();
            playChase = !playChase; 
        }
    }
}
