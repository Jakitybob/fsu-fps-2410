/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* This class creates a scriptable object that allows designers to easily and *
* efficiently create new weapons with ease all using the same set of parameters. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Create an asset menu for designers to easily create new sub objects

public class SO_Weapon : ScriptableObject
{
    //
    // WEAPON ATTRIBUTES
    //

    public GameObject weaponModel;
    public int weaponDamage;
    public float attackRate;
    public int attackRange;
    public int ammoCurrent, ammoMax, totalAmmo; 
    public float reloadTime;
    private bool bCanAttack; // Used to ensure each weapon can attack on their own no matter the coroutine status


    //
    // WEAPON VFX & SFX
    //

    public ParticleSystem hitEffect;
    public AudioClip[] attackSound;
    public float attackVolume;

    
    //
    // GETTERS & SETTERS
    //

    public bool CanAttack() { return bCanAttack; }
    public void SetCanAttack(bool _bCanAttack) { bCanAttack = _bCanAttack; }
    public bool CanReload() { return ammoCurrent < ammoMax; }
}
