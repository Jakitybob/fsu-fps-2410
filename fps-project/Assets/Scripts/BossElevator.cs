using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossElevator : MonoBehaviour
{
    // Start is called before the first frame update
    public float elevatorSpeed = 2f;
    public Transform destination;
    public float bossSpawnDelay = 7f;

    private bool isElevatorActive = false;

    private void Start()
    {
        // Subscribe to the door's "DoorOpened" event
        GameObject.FindGameObjectWithTag("Door").GetComponent<BossDoorScript>().OnDoorOpened += ActivateElevator;
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
            transform.position = Vector3.MoveTowards(transform.position, destination.position, elevatorSpeed * Time.deltaTime);
        }
    }
}
