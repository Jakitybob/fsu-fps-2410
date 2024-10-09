using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Heal : MonoBehaviour
{
    public Healthup healthpickup;
    public void OnTriggerEnter(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if(player!= null)
        {
            healthpickup.applyHeal(player);
            Destroy(gameObject);
        }
        
    }


}
