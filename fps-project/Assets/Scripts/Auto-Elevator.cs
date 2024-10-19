/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Michael Bump *
* *
* This script component implements functioning elevator that automatically goes to its destination as the player steps on. *
************************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevator : MonoBehaviour
{
    public float elevatorSpeed = 2f; // Adjust the speed as needed
    public Transform destination; // Set the destination point for the elevator

    private bool isPlayerOn = false;
    private Transform playerTransform;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = true;
            playerTransform = other.transform;
            playerTransform.parent = transform; // Parent the player to the elevator
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = false;
            playerTransform.parent = null; // Unparent the player from the elevator
        }
    }

    private void Update()
    {
        if (isPlayerOn)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, elevatorSpeed * Time.deltaTime);
        }
    } 

    
    
}
