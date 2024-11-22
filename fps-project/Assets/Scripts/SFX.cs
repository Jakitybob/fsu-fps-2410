using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float defaultVolume = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && SFXControl.Instance != null)
        {
            SFXControl.Instance.SetupExistingAudioSource(audioSource);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }
}
