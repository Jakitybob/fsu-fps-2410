/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* This script component extends the functionality of door.cs to now be locked until *
* the player interacts with it while holding the requisite key card so that the door *
* can become unlocked. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyCardDoor : door, IInteractable
{
    //
    // MEMBER VARIABLES & SERIALIZED FIELDS
    //

    // The color of the key card needed to unlock the door
    [SerializeField] keyCard.keyCardColor doorColor;

    // The lock state of the door, true by default
    private bool isLocked = true;


    //
    // FUNCTIONS
    //

    // Overrides update to make sure the door is unlocked before opening and closing
    void Update()
    {
        if (!isLocked)
        {
            OpenDoor();
        }
    }

    // When interact is called, checks whether or not the door should be unlocked
    public void Interact(Interact interactor)
    {
        // If the interactor has the key card color, unlock the door
        if (interactor.GetKeyCards().Contains(doorColor))
        {
            isLocked = false;
        }
    }
}
