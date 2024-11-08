using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider VolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            LoadVolume();
        
    }
        else
            SetVolume();
        
    }
    private void SetVolume()
    {
       float volume = VolumeSlider.value;  
       Mixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("Music", volume);
    }
    private void LoadVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Music");
        SetVolume();
    }
}
