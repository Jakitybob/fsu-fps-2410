/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* Implements an interactable weapon that can be picked up by the player and added *
* to their list of available weapons. This system utilizes the interactable interface *
* 
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class weaponPickup : MonoBehaviour
{
    //
    // SERIALIZED FIELDS & MEMBER VARIABLES
    //

    [SerializeField] SO_Weapon weapon;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshFilter meshFilter;
    


    //
    // FUNCTIONS
    //

    // Start is called before the first frame update
    void Start()
    {
        // Set the weapon's current ammo to its max
        weapon.ammoCurrent = weapon.ammoMax;
        weapon.SetCanAttack(true);

        // Set the pickup's mesh and texture to match the weapon it is a pickup for
        meshRenderer.sharedMaterial = weapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
        meshFilter.sharedMesh = weapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
    }

    /*
     * If the player wals over this weapon, add it to their arsenal and
     * destroy this pickup object
     */
    private void OnTriggerEnter(Collider other)
    {
        // Check for the player
        if (other.CompareTag("Player"))
        {
            PlayerWeaponComponent comp = other.GetComponent<PlayerWeaponComponent>();
            if (comp != null )
            {
                comp.AddWeaponToList(weapon);
                Destroy(gameObject);
            }
        }
    }
}
