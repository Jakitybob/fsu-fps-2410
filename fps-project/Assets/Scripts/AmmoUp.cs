using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoUp: MonoBehaviour{ 
   [SerializeField] int amount;

    private void OnTriggerEnter(Collider other)
    {
        GunController ammo = other.GetComponent<GunController>();
        if (ammo)
        {
            ammo.AddAmmo(amount); 
            Destroy(gameObject);
        }
        
    }
}
