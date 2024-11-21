using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PersistanceSettings : MonoBehaviour
{
    public static PersistanceSettings Instance { get; private set; }

    [Header("----- UI References -----")]
    [SerializeField] Slider sensSlider;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioMixer audioMixer;

    [System.Serializable]
    private class SettingsData
    {
        public float sensitivity;
        public float volume;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    private void Start()
    {
        // Find and setup references if not set in inspector
        if (sensSlider == null)
            sensSlider = GameObject.Find("SensitivitySlider")?.GetComponent<Slider>();
        
        if (volumeSlider == null)
            volumeSlider = GameObject.Find("VolumeSlider")?.GetComponent<Slider>();

        // Apply loaded settings to UI
        if (sensSlider != null)
            sensSlider.onValueChanged.AddListener(OnSensitivityChanged);
        
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void OnSensitivityChanged(float value)
    {
        var cameraController = FindObjectOfType<cameraController>();
        if (cameraController != null)
        {
            cameraController.sensSlider = sensSlider;
            cameraController.sensChange();
        }
        SaveSettings();
    }

    public void OnVolumeChanged(float value)
    {
        if (audioMixer != null)
        {
            // Convert slider value (0 to 1) to decibels (-80 to 0)
            float dbValue = Mathf.Log10(value) * 20;
            audioMixer.SetFloat("MasterVolume", dbValue);
        }
        SaveSettings();
    }

    public void SaveSettings()
    {
        var settings = new SettingsData
        {
            sensitivity = sensSlider != null ? sensSlider.value : 1f,
            volume = volumeSlider != null ? volumeSlider.value : 1f
        };

        string jsonData = JsonUtility.ToJson(settings);
        PlayerPrefs.SetString("GameSettings", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("GameSettings"))
        {
            string jsonData = PlayerPrefs.GetString("GameSettings");
            var settings = JsonUtility.FromJson<SettingsData>(jsonData);

            // Apply sensitivity
            if (sensSlider != null)
            {
                sensSlider.value = settings.sensitivity;
                OnSensitivityChanged(settings.sensitivity);
            }

            // Apply volume
            if (volumeSlider != null)
            {
                volumeSlider.value = settings.volume;
                OnVolumeChanged(settings.volume);
            }
        }
        else
        {
            // Set default values
            if (sensSlider != null)
            {
                sensSlider.value = 1f;
                OnSensitivityChanged(1f);
            }

            if (volumeSlider != null)
            {
                volumeSlider.value = 1f;
                OnVolumeChanged(1f);
            }
        }
    }
}
