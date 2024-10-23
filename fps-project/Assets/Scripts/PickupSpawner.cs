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
    private int currentIndx;

    void Update()
    {
        spawnTime += Time.deltaTime;

        if(spawnTime >= spawnInterval)
        {
            SpawnPickUp();
            
            spawnTime -=spawnInterval ;  
        }

    }
    public void SpawnPickUp()
    {
        if (PickUP != null && spawnPoint != null)
        {
            GameObject spawned = Instantiate(PickUP, spawnPoint.position, spawnPoint.rotation);
            Healthup heal=spawned.GetComponent<Healthup>();
            AmmoUp ammo = spawned.GetComponent<AmmoUp>();
            if(heal!= null)
            {
                heal.setSpawner(this);
            }
            
            if (ammo != null)
            {
                ammo.setSpawner(this);
            }
        
        }

        
    } 
}
