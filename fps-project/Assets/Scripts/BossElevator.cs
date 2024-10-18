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
    public float bossSpawnDelay = 0.1f;
    public float targetHeight = 10f; // The desired vertical distance to move

    private bool isElevatorActive = false;
    private Vector3 startingPosition;

    private void Start()
    {
        // Subscribe to the door's "DoorOpened" event
        GameObject.FindGameObjectWithTag("Door").GetComponent<TestingDoor>().OnDoorOpened += ActivateElevator;

        // Store the starting position
        startingPosition = transform.position;
    }

    private void ActivateElevator()
    {
        isElevatorActive = true;
        Invoke("SpawnBoss", bossSpawnDelay);
    }

    private void SpawnBoss()
    {
        // Instantiate the boss at the elevator's position
        Instantiate(Resources.Load<GameObject>("BossPrefab"), transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (isElevatorActive)
        {
            // Calculate the target position based on the starting position and target height
            Vector3 targetPosition = startingPosition + new Vector3(0, targetHeight, 0);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, elevatorSpeed * Time.deltaTime);
        }
    }
    
}
