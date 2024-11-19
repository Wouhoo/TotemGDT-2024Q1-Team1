using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        slider.value = VolumeManager.Instance.sfxVolume; // Initialize the slider with the value stored in the volume manager
        slider.onValueChanged.AddListener(UpdateVolume);   // Run UpdateVolume whenever the slider's value changes
    }

    void UpdateVolume(float volume)
    {
        audioSource.volume = volume;                  // Change the assigned audio source's volume
        VolumeManager.Instance.sfxVolume = volume;  // Update the value in the volume manager as well
    }
}
