using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevator : MonoBehaviour
{
    public float elevatorSpeed = 2f; // Adjust the speed as needed
    public Transform destination; // Set the destination point for the elevator

    private bool isPlayerOn = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = false;
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
