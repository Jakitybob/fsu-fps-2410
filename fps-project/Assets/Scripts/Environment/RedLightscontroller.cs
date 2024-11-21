using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightscontroller : MonoBehaviour
{
    public float blinkInterval = 0.5f;
    public Light[] redLights; // Reference to the red light components

    private bool lightsOn = false;
    // Start is called before the first frame update
    void Start()
    {
        // Initially, turn off all the lights
        foreach (Light light in redLights)
        {
            light.intensity = 0f;
        }
    }
    public void TurnLightsOn()
    {
        lightsOn = true;
        StartCoroutine(BlinkLights());
    }

    public void TurnLightsOff()
    {
        lightsOn = false;
        foreach (Light light in redLights)
        {
            light.intensity = 0f;
        }
    }

    private IEnumerator BlinkLights()
    {
        while (lightsOn)
        {
            foreach (Light light in redLights)
            {
                light.intensity = 1f; // Turn on
            }
            yield return new WaitForSeconds(blinkInterval);

            foreach (Light light in redLights)
            {
                light.intensity = 0f; // Turn off
            }
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    
}
