/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* This component is intended to be added to the player character to allow them to *
* interact with the game world. This includes holding inventory and performing *
* ray casts when the interact key is pressed. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    //
    // MEMBER VARIABLES & SERIALIZED FIELDS
    //

    // How far away the player should be able to interact with things
    [SerializeField] float interactionRange;

    // A list of what key cards the player has in their inventory
    private List<keyCard.keyCardColor> keyCards = new List<keyCard.keyCardColor>();


    //
    // FUNCTIONS
    //

    /*
     * Fire a raycast from the player a set distance and check for interactables
     * when a collision is made. If found, call that interactable's interact function.
     */
    public void InteractPressed()
    {
        // Fire a raycast and see if an interactable was hit
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
        {
            //Debug.Log(hit.collider.name);
            // If the collided component has the IInteractable interface, call its interact function
            IInteractable interactee = hit.collider.GetComponent<IInteractable>();
            if (interactee != null)
            {
                interactee.Interact(this); // Pass in this as the interactor
            }
        }
    }

    // Add a key card color to the list of obtained key cards
    public void AddKeyCard(keyCard.keyCardColor addedCard)
    {
        // If the key card is not already in the list, add it
        if (!keyCards.Contains(addedCard))
        {
            keyCards.Add(addedCard);
            // TODO: Implement some kind of UI call to display what key cards the player has?
        }
    }


    //
    // GETTERS & SETTERS
    //

    // Get the list of key cards the player has in their inventory
    public List<keyCard.keyCardColor> GetKeyCards()
    {
        return keyCards;
    }
}
