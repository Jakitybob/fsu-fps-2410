using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthup : MonoBehaviour
{
    public int amount;
    public int respawnTime;
    private PickupSpawner Spawner;
    
   
    public void setSpawner(PickupSpawner spawner)
    {
        this.Spawner = spawner; 

    }
    
    
    private void OnTriggerEnter(Collider other) 
    {
        playerController health = other.GetComponent<playerController>();
        if (health)
        {
            health.Heal(amount);
            if(Spawner != null)
            {
                Spawner.pickedup();
            }
            Destroy(gameObject);
            
        }
        
    }
}