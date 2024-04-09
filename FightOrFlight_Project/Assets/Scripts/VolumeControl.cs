using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; 

    void Start()
    {
        // Load the saved volume value at the start, default to 1 (max volume) if not set
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        AdjustVolume(volumeSlider.value); // Adjust the volume at the start based on saved value
    }

    // This method is called every time the slider value is changed
    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume; // Adjust the master volume
        PlayerPrefs.SetFloat("masterVolume", newVolume); // Save the new volume value
    }
}
