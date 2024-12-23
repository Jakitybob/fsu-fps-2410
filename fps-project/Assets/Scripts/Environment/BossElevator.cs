/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Michael Bump *
* *
* This script component implements functioning elevator that brings the boss up to the playable area after prerequisites are met. *
************************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossElevator : MonoBehaviour
{
    public float elevatorSpeed = 2f;
    
    public float targetHeight = 10f; // The desired vertical distance to move

    private bool isElevatorActive = false;
    private Vector3 startingPosition;

    private void Start()
    {
        // Subscribe to the door's "DoorOpened" event
        GameObject.FindGameObjectWithTag("Door").GetComponent<TestingDoor>().OnDoorOpened += ActivateElevator;
        
        startingPosition = transform.position;
    }

    private void ActivateElevator()
    {
        
        isElevatorActive = true;
        

    }

    

    private void Update()
    {
        if (isElevatorActive)
        {
            // Calculate the target position based on the starting position and target height
            Vector3 targetPosition = startingPosition + new Vector3(0, targetHeight, 0);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, elevatorSpeed * Time.deltaTime);

            foreach (Transform child in transform)
        {
            if (child.CompareTag("Water"))
            {
                child.position = transform.position;
            }
        }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossPrefab"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BossPrefab"))
        {
            other.transform.parent = null;
        }
}
    
}
