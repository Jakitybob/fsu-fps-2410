using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SFXControl : MonoBehaviour
{
    public static SFXControl instance;
    [SerializeField] AudioSource[]SFXobj;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void playSound(AudioClip[] clip, Transform SoundPOS, float volume)
    {
        int order = clip.Length;
        AudioSource audioSource = Instantiate(SFXobj[order], SoundPOS.position, Quaternion.identity);

        audioSource.clip = clip[order];
        audioSource.volume = volume;
        audioSource.Play();
        float length = audioSource.clip.length;
        Destroy(audioSource.gameObject,length);

    }
}


