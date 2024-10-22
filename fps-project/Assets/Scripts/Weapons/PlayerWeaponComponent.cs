/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* This class implements a once-separated controller for the player's weaponry, *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponComponent : MonoBehaviour
{
    //
    // SERIALIZED FIELDS & MEMBER VARIABLES
    //

    [SerializeField] LayerMask ignoreMask;

    [SerializeField] List<SO_Weapon> weaponList;
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject weaponAttackEffect; // Effect for things like muzzle flash or slice of air from swing

    private int weaponIndex;
    private int weaponDamage;
    private float weaponAttackRate;
    private int weaponRange;


    //
    // FUNCTIONS
    //

    /*
     * If the player has a weapon with enough ammo, and that weapon can attack,
     * attack with that weapon.
     */
    public void WeaponAttack()
    {
        if (weaponList.Count > 0 && weaponList[weaponIndex].ammoCurrent > 0 && weaponList[weaponIndex].CanAttack())
        {
            StartCoroutine(DoWeaponEffect());
            StartCoroutine(Attack());
        }
    }

    /*
     * Sets the current weapon to attacking, decrements ammo, and performs a raycast
     * to check for a damageable actor and, if one is found, calls takeDamage on the actor.
     * Yields for the current weapon's fireRate time before making that weapon able to attack again.
     */
    IEnumerator Attack()
    {
        // Sett the character to now attacking
        weaponList[weaponIndex].SetCanAttack(false);
        weaponList[weaponIndex].ammoCurrent -= 1;
        // TODO: Update ammo in HUD here

        // Fire a raycast using the Physics class and check for damageable objects
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponRange, ~ignoreMask))
        {
            Debug.Log(hit.collider.name); // Debug text to make sure the raycast is hittig proper objects

            // Check if the hit object can be damaged via the IDamage interface
            IDamage damage = hit.collider.GetComponent<IDamage>();

            // No matter the hit result, instantiate the weapon's hit effect
            //Instantiate(weaponList[weaponIndex].hitEffect, hit.point, Quaternion.identity);

            // Damage the hit actor if it was found
            if (damage != null)
            {
                damage.takeDamage(weaponDamage);
            }
        }

        // Wait to be able to fire again
        yield return new WaitForSeconds(weaponAttackRate);
        weaponList[weaponIndex].SetCanAttack(true);
    }

    /*
     * Sets the weapon effect to active for a tenth of a second before
     * deactivating it again.
     */
    IEnumerator DoWeaponEffect()
    {
        weaponAttackEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f); // Probably should re-evaluate this entire function tbh
        weaponAttackEffect.SetActive(false);
    }

    /*
     * Reloads the current weapon if it can be reloaded.
     * TODO: Implement a total ammo for any given weapon for ammo pickups and only allow reloads if
     * the player has enough ammo stored.
     */
    public void Reload()
    {
        // Only perform a reload if it would actually do anything
        if (weaponList[weaponIndex].ammoCurrent < weaponList[weaponIndex].ammoMax)
        {
            StartCoroutine(DoReload());
            weaponList[weaponIndex].ammoCurrent = weaponList[weaponIndex].ammoMax;
        }
    }

    /*
     * Stops the gun from shooting while reloading, and should eventually also fire the animation
     * for reloading on the gun and simply wait for that to finish before allowing the weapon
     * to attack again.
     */
    IEnumerator DoReload()
    {
        // TODO: Implement playing a reload animation here
        weaponList[weaponIndex].SetCanAttack(false);
        yield return new WaitForSeconds(weaponList[weaponIndex].reloadTime);
        weaponList[weaponIndex].SetCanAttack(true);
    }

    /*
     * Change the currently equipped weapon to the weapon input
     * to this function.
     * 
     * @param weapon: an SO_Weapon object to apply stats from.
     */
    public void SetWeaponStats(SO_Weapon weapon)
    {
        // Change the weapon stats
        weaponDamage = weapon.weaponDamage;
        weaponAttackRate = weapon.attackRate;
        weaponRange = weapon.attackRange;

        // Change the weapon's visual appearance
        weaponModel.GetComponent<MeshFilter>().sharedMesh = weapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    /*
     * Adds the passed in weapon to the character's weapon list.
     * 
     * @param weapon: an SO_Weapon to add to the list.
     */
    public void AddWeaponToList(SO_Weapon weapon)
    {
        // Add the weapon to the list and equip it
        weaponList.Add(weapon);
        weaponIndex = weaponList.Count - 1;
        SetWeaponStats(weapon);
        gameManager.instance.playerScript.updatePlayerUI(); // Update the player UI when picking up a weapon
    }


    //
    // GETTERS & SETTERS
    //

    public int GetWeaponIndex() { return weaponIndex; }
    public void SetWeaponIndex(int index)
    {
        // If the index would be out of range simply return
        if (index < 0 || index >= weaponList.Count)
        {
            return;
        }
        // Otherwise set the weaponIndex to the index specified
        else
            weaponIndex = index;
    }

    public SO_Weapon GetCurrentWeapon() { return weaponList[weaponIndex]; }
    public SO_Weapon GetWeaponAtIndex(int index)
    {
        // If the weapon would be out of bounds simply return null
        if (index < 0 || index >= weaponList.Count)
            return null;
        // Otherwise return the weapon at the index specified
        else
            return weaponList[index];
    }

    public int GetCurrentAmmo() { return weaponList[weaponIndex].ammoCurrent; }
    public int GetTotalAmmo() { return weaponList[weaponIndex].ammoMax; }

    public int GetWeaponDamage() { return weaponDamage; }
    
    public float GetWeaponAttackRate() { return weaponAttackRate; }
    
    public int GetWeaponRange() { return weaponRange; }

    public int GetWeaponListCount() { return weaponList.Count; }
}
