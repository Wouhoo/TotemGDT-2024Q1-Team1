using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    // the themes have an intro part and a loop part, so I need 2 audio players
    private AudioSource audioPlayer;

    //is the hidden theme playing?
    private bool player_hidden = true;
    //flag used to override sheduled loop playing
    private string currentTheme; // Tracks the current theme ("sneak" or "chase" or "menu" - given I will make a menu theme lolol)
    //[SerializeField] AudioSource audioIntroPlayer;

    [SerializeField] AudioClip hiddenThemeIntro;
    [SerializeField] AudioClip hiddenThemeLoop;
    [SerializeField] AudioClip chaseThemeIntro;
    [SerializeField] AudioClip chaseThemeLoop;

    void OnEnable()
    {
        // Subscribe to the PlayerSpotted event
        State_Pursue.OnPlayerSpotted += evPlayerSpotted;
        State_Pursue.OnLostPlayer += evPlayerLost;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        State_Pursue.OnPlayerSpotted -= evPlayerSpotted;
        State_Pursue.OnLostPlayer -= evPlayerLost;
    }


    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.volume = VolumeManager.Instance.musicVolume;
        PlaySneakingThemeWithLoop();
    }


    public void PlaySneakingThemeWithLoop()
    {
        if (currentTheme == "sneak") return; // Avoid re-triggering
        CancelInvoke(nameof(PlayChaseThemeLoop)); //cancel the loop invoke
        audioPlayer.Stop();
        audioPlayer.loop = false;
        // Set the intro clip
        audioPlayer.clip = hiddenThemeIntro;
        currentTheme = "sneak";
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


    public void PlayChaseThemeWithLoop()
    {
        if (currentTheme == "chase") return; // Avoid re-triggering
        CancelInvoke(nameof(PlaySneakingLoop)); //cancel the loop invoke
        //only used for hidden theme, chase theme can't be overridden
        audioPlayer.Stop();
        audioPlayer.loop = false;
        // Set the intro clip
        audioPlayer.clip = chaseThemeIntro;
        currentTheme = "chase";
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
            PlayChaseThemeWithLoop();
        }
    }

    public void evPlayerLost()
    {
        player_hidden = true;
        PlaySneakingThemeWithLoop();
    }

    // DEBUG - Switch between themes by pressing backslash
    // In the actual theme, the chase theme should start playing when *any* enemy spots the player,
    // and the hidden theme should come back when *all* enemies have lost sight of the player.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (player_hidden)
                PlayChaseThemeWithLoop();
            else
                PlaySneakingThemeWithLoop();
            player_hidden = !player_hidden;
        }
    }
}
