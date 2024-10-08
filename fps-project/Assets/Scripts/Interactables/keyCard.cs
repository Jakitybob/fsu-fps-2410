/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* Implements the key card game object that will be used to open locked, color-coded *
* doors within the game. Key cards are picked up by running over them, much like in *
* the original DOOM. Once aquired they can be used on all doors of the same color *
* as the key card for the duration of the level. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyCard : MonoBehaviour, IInteractable
{
    //
    // MEMBER VARIABLES & SERIALIZED FIELDS
    //

    // An enumator for the color of the key card, used to pair keycards to colored doors
    public enum keyCardColor { blue, red, yellow }
    [SerializeField] keyCardColor cardColor;

    // The speed at which the key card should rotate
    [SerializeField] float rotationSpeed;


    //
    // FUNCTIONS
    //

    // Rotates the key card around the y-axis
    void Update()
    {
        // Rotate the key card at the set speed around the y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    /*
     * When the trigger for the game object has been entered, if the entering
     * game object has the interact component, then call interact. Otherwise,
     * do nothing and continue spinning :)
     */
    private void OnTriggerEnter(Collider other)
    {
        // If the collider is another trigger, return. Just here to avoid strange Unity interactions
        if (other.isTrigger)
            return;

        // If the player 
        Interact playerInteractor = other.GetComponent<Interact>();
        if (playerInteractor != null)
        {
            Interact(playerInteractor);
        }
    }

    /*
     * Adds the key card to the interactor's key card list, then destroys itself.
     */
    public void Interact(Interact interactor)
    {
        // Add the key card to the player's inventory of cards
        interactor.AddKeyCard(cardColor);

        // Destroy the object as this key card is no longer needed
        Destroy(gameObject);
    }
}
