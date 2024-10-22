using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoUp: MonoBehaviour{ 
   [SerializeField] int amount;

    private void OnTriggerEnter(Collider other)
    {

        PlayerWeaponComponent weapon = other.GetComponent<PlayerWeaponComponent>();
        
        if (weapon != null)
        {//current weapon inplayer hand
           SO_Weapon currWeapon = weapon.GetCurrentWeapon();
            //make sure its valid
            if (currWeapon != null && currWeapon.totalAmmo < currWeapon.ammoMax) {
                
                currWeapon.totalAmmo = Mathf.Min(currWeapon.totalAmmo + amount, currWeapon.ammoMax);
            }
            Destroy(gameObject);
        }
        
    }
}
