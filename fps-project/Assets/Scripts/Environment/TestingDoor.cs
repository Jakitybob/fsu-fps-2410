/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Michael Bump *
* *
* this simply a test door for the boss elevator to function *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingDoor : MonoBehaviour, IInteractable
{
    public delegate void DoorOpenedHandler();
    public event DoorOpenedHandler OnDoorOpened;
    public float doorSpeed = 2f;
    public float openAngle = 90f;

    private bool isOpen = false;
    private Quaternion closedRotation;

    void Start()
    {
        closedRotation = transform.rotation;
    }

    void Update()
    {
        /* if (isOpen)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation * Quaternion.Euler(0, openAngle, 0), doorSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, doorSpeed * Time.deltaTime);
        } */
        if (isOpen)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation * Quaternion.Euler(0, openAngle, 0), doorSpeed * Time.deltaTime);

            // Check if the door is fully open
            if (Quaternion.Angle(transform.rotation, closedRotation * Quaternion.Euler(0, openAngle, 0)) < 1f)
            {
                isOpen = false; // Prevent further rotation
                OnDoorOpened?.Invoke(); // Trigger the event
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, doorSpeed * Time.deltaTime);
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    public void Interact(Interact interactor)
    {
        ToggleDoor();
    }

    private void DoorOpened()
    {
        
        if (OnDoorOpened != null)
        {
            OnDoorOpened();
        }
    }
}
