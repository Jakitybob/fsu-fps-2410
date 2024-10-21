using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthup : MonoBehaviour
{
    public int amount;
    //sets heal amount and appplies it   
    private void OnTriggerEnter(Collider other) 
    {
        playerController health = other.GetComponent<playerController>();
        if (health)
        {
            health.Heal(amount);
            Destroy(gameObject); 
        }
        
    }
}