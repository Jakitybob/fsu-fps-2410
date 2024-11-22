using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXControl : MonoBehaviour
{
    public static SFXControl Instance { get; private set; }
    
    [SerializeField] AudioSource SFXobj;
    [SerializeField] AudioMixerGroup sfxMixerGroup;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure we have a reference to the SFX mixer group
        if (sfxMixerGroup == null)
        {
            var mixerManager = MixerManager.Instance;
            if (mixerManager != null)
            {
                sfxMixerGroup = mixerManager.GetSFXGroup();
                Debug.Log("SFXControl: Got SFX mixer group from MixerManager");
            }
            else
            {
                Debug.LogWarning("SFXControl: MixerManager not found! SFX volume control won't work.");
            }
        }

        // Make sure the SFXobj prefab has the correct mixer group
        if (SFXobj != null && sfxMixerGroup != null)
        {
            SFXobj.outputAudioMixerGroup = sfxMixerGroup;
            Debug.Log("SFXControl: Set mixer group on SFXobj prefab");
        }
    }

    public void playSound(AudioClip clip, Transform soundPos, float volume)
    {
        if (clip == null || soundPos == null) 
        {
            Debug.LogWarning("SFXControl: Tried to play null clip or at null position");
            return;
        }

        AudioSource audioSource = Instantiate(SFXobj, soundPos.position, Quaternion.identity);
        
        // Always ensure the audio source is connected to the SFX mixer group
        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning("SFXControl: No SFX mixer group assigned! Volume control won't work.");
        }

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        
        float length = audioSource.clip.length;
        Destroy(audioSource.gameObject, length);
    }

    // Overload for playing sounds at camera position (UI sounds)
    public void playSound(AudioClip clip, float volume)
    {
        if (clip == null || Camera.main == null) return;
        playSound(clip, Camera.main.transform, volume);
    }

    // Call this to ensure any existing AudioSource in the scene uses the SFX mixer group
    public void SetupExistingAudioSource(AudioSource source)
    {
        if (source != null && sfxMixerGroup != null)
        {
            source.outputAudioMixerGroup = sfxMixerGroup;
            Debug.Log($"SFXControl: Set mixer group on existing AudioSource: {source.gameObject.name}");
        }
    }
}
