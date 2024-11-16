/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Michael Bump *
* *
* this simply a test door for the boss elevator to function *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingDoor : MonoBehaviour
{
    public delegate void DoorOpenedHandler();
    public event DoorOpenedHandler OnDoorOpened;
    public float doorSpeed = 2f;

    public float openDistance = 2f;

    private bool isOpen = false;
    private Vector3 closedPosition;
    public bool useTrigger = true;
    public Collider triggerCollider;


    void Start()
    {
        closedPosition = transform.position;
        triggerCollider.gameObject.SetActive(true);
    }

    void Update()
    {

        /* if (isOpen)
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
        } */
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition + Vector3.up * openDistance, doorSpeed * Time.deltaTime);

            // Check if the door is fully open
            if (Vector3.Distance(transform.position, closedPosition + Vector3.up * openDistance) < 0.1f)
            {
                isOpen = false; // Prevent further movement
                OnDoorOpened?.Invoke(); // Trigger the event
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, doorSpeed * Time.deltaTime);
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }


    
    private void DoorOpened()
    {

        if (OnDoorOpened != null)
        {
            OnDoorOpened();
        }
    }
}
