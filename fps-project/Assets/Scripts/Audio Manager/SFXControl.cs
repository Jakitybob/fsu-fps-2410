using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SFXControl : MonoBehaviour
{
    public static SFXControl instance;
    [SerializeField] AudioSource SFXobj;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void playSound(AudioClip clip, Transform SoundPOS, float volume)
    {
        
        AudioSource audioSource = Instantiate(SFXobj, SoundPOS.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        
        float length = audioSource.clip.length;
        Destroy(audioSource.gameObject,length);

    }
}


