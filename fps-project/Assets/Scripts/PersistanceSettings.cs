using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PersistanceSettings : MonoBehaviour
{
    public static PersistanceSettings Instance { get; private set; }

    [Header("----- UI References -----")]
    [SerializeField] Slider mouseSensSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    private MixerManager mixerManager;
    private cameraController activeCameraController;
    private bool isInitialized = false;
    private SettingsData currentSettings;

    [System.Serializable]
    public class SettingsData
    {
        public float mouseSensitivity = 1f;
        public float sfxVolume = 1f;
        public float musicVolume = 1f;
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

        // Initialize settings data
        if (PlayerPrefs.HasKey("GameSettings"))
        {
            string jsonData = PlayerPrefs.GetString("GameSettings");
            currentSettings = JsonUtility.FromJson<SettingsData>(jsonData);
        }
        else
        {
            currentSettings = new SettingsData();
            SaveSettings();
        }

        // Subscribe to scene loading event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PersistanceSettings] Scene loaded: {scene.name}, Mode: {mode}");
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        // Wait two frames to ensure all objects are initialized
        yield return null;
        yield return null;

        // Find references
        FindReferences();

        // Apply saved settings
        ApplySettings(currentSettings);

        isInitialized = true;
        Debug.Log("[PersistanceSettings] Settings initialized after scene load");
    }

    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private void FindReferences()
    {
        Debug.Log("[PersistanceSettings] Finding references...");
        
        // Find references if not set
        if (mouseSensSlider == null)
            mouseSensSlider = GameObject.Find("MouseSensitivitySlider")?.GetComponent<Slider>();
        
        if (sfxVolumeSlider == null)
            sfxVolumeSlider = GameObject.Find("SFXVolumeSlider")?.GetComponent<Slider>();

        if (musicVolumeSlider == null)
            musicVolumeSlider = GameObject.Find("MusicVolumeSlider")?.GetComponent<Slider>();

        // Get MixerManager reference
        mixerManager = MixerManager.Instance;

        // Setup listeners
        SetupSliderListeners();
    }

    private void SetupSliderListeners()
    {
        if (mouseSensSlider != null)
        {
            mouseSensSlider.onValueChanged.RemoveAllListeners();
            mouseSensSlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
    }

    private void ApplySettings(SettingsData settings)
    {
        Debug.Log($"[PersistanceSettings] Applying settings - Mouse: {settings.mouseSensitivity}, SFX: {settings.sfxVolume}, Music: {settings.musicVolume}");

        // Apply audio settings first
        if (mixerManager != null)
        {
            mixerManager.setSFXVol(settings.sfxVolume);
            mixerManager.setMusicVol(settings.musicVolume);
        }

        // Update UI without triggering events
        if (mouseSensSlider != null)
        {
            mouseSensSlider.value = settings.mouseSensitivity;
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = settings.sfxVolume;
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = settings.musicVolume;
        }

        // Update camera sensitivity
        activeCameraController = FindObjectOfType<cameraController>();
        if (activeCameraController != null)
        {
            activeCameraController.SetSensitivity(settings.mouseSensitivity);
        }

        SetupSliderListeners();
    }

    public void OnMouseSensitivityChanged(float value)
    {
        currentSettings.mouseSensitivity = value;
        activeCameraController = FindObjectOfType<cameraController>();
        if (activeCameraController != null)
        {
            activeCameraController.SetSensitivity(value);
        }
        SaveSettings();
    }

    public void OnSFXVolumeChanged(float value)
    {
        currentSettings.sfxVolume = value;
        if (mixerManager == null)
            mixerManager = MixerManager.Instance;

        if (mixerManager != null)
        {
            mixerManager.setSFXVol(value);
        }
        SaveSettings();
    }

    public void OnMusicVolumeChanged(float value)
    {
        currentSettings.musicVolume = value;
        if (mixerManager == null)
            mixerManager = MixerManager.Instance;

        if (mixerManager != null)
        {
            mixerManager.setMusicVol(value);
        }
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(currentSettings);
        PlayerPrefs.SetString("GameSettings", jsonData);
        PlayerPrefs.Save();
        Debug.Log($"[PersistanceSettings] Settings saved - Mouse: {currentSettings.mouseSensitivity}, SFX: {currentSettings.sfxVolume}, Music: {currentSettings.musicVolume}");
    }
}
