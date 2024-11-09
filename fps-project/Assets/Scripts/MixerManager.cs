using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void setMasterVol(float level)
    {
        mixer.SetFloat("Master", Mathf.Log10(level) * 20);

    }
    public void setSFXVol(float level)
    {
        mixer.SetFloat("SFX", Mathf.Log10(level) * 20);


    }
    public void setMusicVol(float level)
    {
        mixer.SetFloat("Music", Mathf.Log10(level) * 20);

    }
}
