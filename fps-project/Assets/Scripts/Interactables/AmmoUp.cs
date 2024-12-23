using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoUp: MonoBehaviour
{ 
   [SerializeField] int amount;
    public PickupSpawner spawner;

    

    public void setSpawner(PickupSpawner spawner)
    {
        this.spawner = spawner;
    }
    
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerWeaponComponent weapon = other.GetComponent<PlayerWeaponComponent>();
        
        if (weapon != null)
        {//current weapon inplayer hand
           SO_Weapon currWeapon = weapon.GetCurrentWeapon();
            //make sure its valid
            if (currWeapon != null) {
                
                currWeapon.totalAmmo += Mathf.Min(currWeapon.totalAmmo + amount, currWeapon.ammoMax); // Add a full magazine worth of ammo to the equipped weapon
                gameManager.instance.playerScript.updatePlayerUI(); // Update the player UI to reflect the new ammo
            }
            if(spawner!= null)
            {
                spawner.pickedup(); 
            }
            Destroy(gameObject);
        }
    }
}
