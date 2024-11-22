/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates * Gyoed Crespo * Z Broyles
* *
* A brief description of the program should also be added here. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour
{
    //
    // SERIALIZED FIELDS
    //

    [SerializeField] float sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] float maxSens = 5f;  // Default max sensitivity
    [SerializeField] float minSens = 0.1f;  // Default min sensitivity

    //
    // MEMBER VARIABLES
    //

    float rotationX;
    public Slider sensSlider;
    private float currentSensitivityValue = 1f;
    private bool isInitialized = false;

    //
    // FUNCTIONS
    //

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Try to find the sensitivity slider if not already set
        if (sensSlider == null)
        {
            sensSlider = GameObject.Find("MouseSensitivitySlider")?.GetComponent<Slider>();
            if (sensSlider != null)
            {
                Debug.Log($"[CameraController] Found sensitivity slider with value: {sensSlider.value}");
                currentSensitivityValue = sensSlider.value;
                sensChange();
            }
            else
            {
                Debug.LogWarning("[CameraController] Could not find MouseSensitivitySlider!");
            }
        }

        // Initialize sensitivity
        if (!isInitialized)
        {
            // Try to get settings from PersistanceSettings
            var persistanceSettings = FindObjectOfType<PersistanceSettings>();
            if (persistanceSettings != null)
            {
                Debug.Log("[CameraController] Found PersistanceSettings, waiting for initialization...");
                StartCoroutine(WaitForSettings());
            }
            else
            {
                Debug.LogWarning("[CameraController] PersistanceSettings not found, using default sensitivity.");
                SetSensitivity(1f);
            }
        }
    }

    private IEnumerator WaitForSettings()
    {
        yield return new WaitForSeconds(0.1f); // Give PersistanceSettings time to initialize
        if (!isInitialized && sensSlider != null)
        {
            Debug.Log($"[CameraController] Initializing with slider value: {sensSlider.value}");
            SetSensitivity(sensSlider.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update sensitivity if slider exists and value changed
        if (sensSlider != null && currentSensitivityValue != sensSlider.value)
        {
            currentSensitivityValue = sensSlider.value;
            sensChange();
        }
        
        // Get the input from the x and y axes of the mouse
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        // Handle camera inversion
        if (!invertY)
        {
            rotationX -= mouseY;
        }
        else
        {
            rotationX += mouseY;
        }

        // Clamp the rotationX on the x-axis within specified parameters
        rotationX = Mathf.Clamp(rotationX, lockVertMin, lockVertMax);

        // Rotate the camera about the x-axis (up and down)
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Turn the player about the y-axis (left and right)
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    public void sensChange()
    {
        if (sensSlider == null) return;

        float newSens = sensSlider.value * maxSens;
        sens = Mathf.Max(newSens, minSens);
        Debug.Log($"[CameraController] Sensitivity changed to: {sens} (slider: {sensSlider.value})");
    }

    // Method to set sensitivity directly (used by PersistanceSettings)
    public void SetSensitivity(float value)
    {
        currentSensitivityValue = value;
        if (sensSlider != null)
        {
            sensSlider.value = value;
        }
        float newSens = value * maxSens;
        sens = Mathf.Max(newSens, minSens);
        isInitialized = true;
        Debug.Log($"[CameraController] SetSensitivity called with value: {value}, final sens: {sens}");
    }
}
