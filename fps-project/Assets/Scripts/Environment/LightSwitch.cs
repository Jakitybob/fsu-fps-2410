using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public RedLightscontroller redLightController;
    private bool isLightOn = true;

    void Start()
    {
        // Ensure lights start in the on state
        redLightController.TurnLightsOn();
    }

    public void Interact(Interact interactor)
    {
        ToggleLights();
    }

    private void ToggleLights()
    {
        isLightOn = !isLightOn;
        if (isLightOn)
        {
            redLightController.TurnLightsOn();
        }
        else
        {
            redLightController.TurnLightsOff();
        }
    }
}
