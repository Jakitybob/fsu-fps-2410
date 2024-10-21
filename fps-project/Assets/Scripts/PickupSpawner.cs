using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject PickUP;
    public Transform spawnPoint;

    public float spawnInterval;
    private float spawnTime;

    void Update()
    {
        spawnTime += Time.deltaTime;

        if(spawnTime >= spawnInterval)
        {
            SpawnPickUp();
            
            spawnTime = 0;  
        }

    }
    void SpawnPickUp()
    {
        Instantiate(PickUP, spawnPoint);
        
    } 
}
