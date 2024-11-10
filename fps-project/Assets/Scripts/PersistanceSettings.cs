using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistanceSettings : MonoBehaviour
{
    PersistanceSettings Instance;
    Slider sensSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        loadSens();
    }

    public void updateSens()
    {
        cameraController cameraController = FindObjectOfType<cameraController>();

        cameraController.sensSlider = sensSlider;
        cameraController.sensChange();
    }

    public void saveSens()
    {
        string jsonData = JsonUtility.ToJson(sensSlider);
        PlayerPrefs.SetString("sensData", jsonData);
        PlayerPrefs.Save();
    }

    public void loadSens()
    {
        // Implement deserialization logic to load the Items list from a file or player preferences
        if (PlayerPrefs.HasKey("sensData"))
        {
            string jsonData = PlayerPrefs.GetString("sensData");
            sensSlider = JsonUtility.FromJson<Slider>(jsonData);
        }
    }
}
