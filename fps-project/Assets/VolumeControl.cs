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
    [SerializeField] private Slider SFXslider;

    private void Start()
    {
        float CurrVolm;
        if(Mixer.GetFloat("Music",out CurrVolm))
        {
            VolumeSlider.value = MathF.Pow(10, CurrVolm / 20);
        }
        VolumeSlider.onValueChanged.AddListener(onSlide);
    }
    private void onSlide(float value)
    {
        float volume = MathF.Log10(Mathf.Clamp(value, 0.0f, 1f)) * 20;
        Mixer.SetFloat("Music", volume);
    }
    private void onSFXSlide(float value)
    {
        float volume = MathF.Log10(Mathf.Clamp(value, 0.0f, 1f)) * 20;
        Mixer.SetFloat("SFX", volume);
    }
    private void OnDestroy()
    {
        VolumeSlider.onValueChanged.RemoveListener(onSlide);
        SFXslider.onValueChanged.RemoveListener(onSFXSlide);
    }

}
