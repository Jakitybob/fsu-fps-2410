using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFOV : MonoBehaviour
{

    private Camera playerCamera;
    private float desiredFOV;
    private float currFOV;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        desiredFOV = playerCamera.fieldOfView;
        currFOV = playerCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        float fovSpeed = 4f;
        currFOV = Mathf.Lerp(currFOV, desiredFOV, Time.deltaTime * fovSpeed);
        playerCamera.fieldOfView = currFOV;
    }


    public void setFOV(float newFOV)
    {
        desiredFOV = newFOV;
    }
}
