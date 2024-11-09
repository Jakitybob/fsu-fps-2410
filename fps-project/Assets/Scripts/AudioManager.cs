using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SfxSource;

    [Header("Sounds")]
    public AudioClip Background;
    public AudioClip Menu;
    public AudioClip Fighting;
    public AudioClip Kill;
    public AudioClip Keycard;
    public AudioClip Gun;
    public AudioClip Load;
    public AudioClip Win;
    public AudioClip Lose;


    private void Start()
    {
        MusicSource.clip = Background;
        MusicSource.Play();
    }

    public void playSfx(AudioClip sound)
    {
        SfxSource.PlayOneShot(sound);

    }
}
