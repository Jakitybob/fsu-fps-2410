using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volum = "Master";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider volumeSlider;
    private float Multiplier = 10;
    [SerializeField]Toggle volumeToggle;
    private bool volumUntoggle;

    // Start is called before the first frame update


    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(slidervalueChange);
        volumeToggle.onValueChanged.AddListener(Toggled);
        
    }

    private void Toggled(bool enabled)
    {
        if (volumUntoggle) return;
        if (enabled)
        {
            volumeSlider.value = volumeSlider.maxValue;
        }
        else
        {
            volumeSlider.value = volumeSlider.minValue;
        }
        
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volum, volumeSlider.value);
        
    }

    private void slidervalueChange(float volume)
    {
        mixer.SetFloat(volum, Mathf.Log10(volume)* Multiplier);
        volumUntoggle = true;
        volumeToggle.isOn = volumeSlider.value > volumeSlider.minValue;
        volumUntoggle = false;

    }

    void Start()
    {
        volumeSlider.value=PlayerPrefs.GetFloat(volum, volumeSlider.value);
        
    }

}
