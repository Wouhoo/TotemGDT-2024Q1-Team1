using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    // the themes have an intro part and a loop part, so I need 2 audio players
    private AudioSource audioPlayer;

    private bool player_hidden = true;
    //[SerializeField] AudioSource audioIntroPlayer;

    [SerializeField] AudioClip hiddenThemeIntro;
    [SerializeField] AudioClip hiddenThemeLoop;
    [SerializeField] AudioClip chaseThemeIntro;
    [SerializeField] AudioClip chaseThemeLoop;

    void OnEnable()
    {
        // Subscribe to the PlayerSpotted event
        State_Pursue.OnPlayerSpotted += evPlayerSpotted;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        State_Pursue.OnPlayerSpotted -= evPlayerSpotted;
    }


    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.volume = VolumeManager.Instance.musicVolume;
        PlaySneakingThemeWithLoop();
    }


    public void PlaySneakingThemeWithLoop()
    {
        audioPlayer.loop = false;
        // Set the intro clip
        audioPlayer.clip = hiddenThemeIntro;

        // Play the intro clip
        audioPlayer.Play();

        // Schedule switching to the loop
        Invoke(nameof(PlaySneakingLoop), hiddenThemeIntro.length);
    }

    private void PlaySneakingLoop()
    {
        // Switch to the loop clip
        audioPlayer.clip = hiddenThemeLoop;
        audioPlayer.loop = true; // Enable looping for the loop clip
        audioPlayer.Play();
    }


    public void PlayChaseThemeThemeWithLoop()
    {
        audioPlayer.loop = false;
        // Set the intro clip
        audioPlayer.clip = chaseThemeIntro;

        // Play the intro clip
        audioPlayer.Play();

        // Schedule switching to the loop
        Invoke(nameof(PlayChaseThemeLoop), chaseThemeIntro.length);
    }

    private void PlayChaseThemeLoop()
    {
        // Switch to the loop clip
        audioPlayer.clip = chaseThemeLoop;
        audioPlayer.loop = true; // Enable looping for the loop clip
        audioPlayer.Play();
    }


    public void evPlayerSpotted()
    {

        if (player_hidden)
        {
            player_hidden = false;
            PlayChaseThemeThemeWithLoop();
        }
    }

    // DEBUG - Switch between themes by pressing backslash
    // In the actual theme, the chase theme should start playing when *any* enemy spots the player,
    // and the hidden theme should come back when *all* enemies have lost sight of the player.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (player_hidden)
                PlayChaseThemeThemeWithLoop();
            else
                PlaySneakingThemeWithLoop();
            player_hidden = !player_hidden;
        }
    }
}
