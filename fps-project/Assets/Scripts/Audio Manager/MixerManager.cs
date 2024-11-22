using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MixerManager : MonoBehaviour
{
    public static MixerManager Instance { get; private set; }

    [SerializeField] AudioMixer mixer;

    private float currentSFXVol = 1f;
    private float currentMusicVol = 1f;
    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene loading event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Initialize mixer with default values
        InitializeMixer();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[MixerManager] Scene loaded: {scene.name}, Mode: {mode}");
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        // Wait two frames to ensure all objects are initialized
        yield return null;
        yield return null;

        // Reapply current volume settings
        ApplyCurrentVolumes();
        isInitialized = true;
        Debug.Log("[MixerManager] Audio settings reapplied after scene load");
    }

    private void InitializeMixer()
    {
        if (mixer == null)
        {
            Debug.LogError("[MixerManager] Audio Mixer not assigned!");
            return;
        }

        // Set initial volumes
        ApplyCurrentVolumes();
        isInitialized = true;
    }

    private void ApplyCurrentVolumes()
    {
        if (mixer == null) return;

        // Convert linear volume to logarithmic dB
        float sfxDB = Mathf.Log10(Mathf.Max(0.0001f, currentSFXVol)) * 20f;
        float musicDB = Mathf.Log10(Mathf.Max(0.0001f, currentMusicVol)) * 20f;

        mixer.SetFloat("SFXVolume", sfxDB);
        mixer.SetFloat("MusicVolume", musicDB);

        Debug.Log($"[MixerManager] Applied volumes - SFX: {currentSFXVol} ({sfxDB}dB), Music: {currentMusicVol} ({musicDB}dB)");
    }

    public void setSFXVol(float volume)
    {
        currentSFXVol = volume;
        if (mixer != null)
        {
            float dB = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20f;
            mixer.SetFloat("SFXVolume", dB);
            Debug.Log($"[MixerManager] Set SFX volume to {volume} ({dB}dB)");
        }
    }

    public void setMusicVol(float volume)
    {
        currentMusicVol = volume;
        if (mixer != null)
        {
            float dB = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20f;
            mixer.SetFloat("MusicVolume", dB);
            Debug.Log($"[MixerManager] Set Music volume to {volume} ({dB}dB)");
        }
    }

    public AudioMixerGroup GetSFXGroup()
    {
        if (mixer == null) return null;
        return mixer.FindMatchingGroups("SFX")[0];
    }

    public AudioMixerGroup GetMusicGroup()
    {
        if (mixer == null) return null;
        return mixer.FindMatchingGroups("Music")[0];
    }
}
