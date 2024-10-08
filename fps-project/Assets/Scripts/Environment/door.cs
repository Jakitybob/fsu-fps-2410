/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* This script component implements doors that automatically open as the player *
* approaches their vicinity. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class door : MonoBehaviour
{
    //
    // MEMBER VARIABLES & SERIALIZED FIELDS
    //

    // How fast the door should open
    [SerializeField] float doorSpeed;

    // How far the door should open in the y-direction
    [SerializeField] float openHeight;

    // The original position and open position of the door
    private Vector3 closedPosition;
    private Vector3 openPosition;

    // Whether the door is open or closed
    private bool isOpen = false;


    //
    // FUNCTIONS
    //

    // Start is called before the first frame update
    void Start()
    {
        // Set the closed position to the current position of the door
        closedPosition = transform.position;

        // Set the open position to the closed position + the open height
        openPosition = closedPosition;
        openPosition.y += openHeight;
    }

    // Update is called once per frame
    void Update()
    {
        OpenDoor();
    }

    /*
     * On trigger enter, the door should open no matter what entered. Ideally the trigger's layer
     * mask will be set to ignore anything but players and enemies.
     */
    private void OnTriggerEnter(Collider other)
    {
        // If the collider is another trigger, simply return
        if (other.isTrigger)
            return;

        // Mark the door as open and let Update() do the rest
        isOpen = true;
    }

    /*
     * On trigger exit, the door should close no matter what entered. Ideally the trigger's layer
     * mask will be set to ignore anything but players and enemies.
     */
    private void OnTriggerExit(Collider other)
    {
        // If the collider is another trigger, simply return
        if (other.isTrigger)
            return;

        // TODO: check to make sure this is the last thing in range of the door, so as not to shut out things still in range
        // Mark the door as closed and let Update() do the rest
        isOpen = false;
    }

    // Logic for opening and closing the door physically - to be called in Update()
    public void OpenDoor()
    {
        // If the door is open but has not completed opening, lerp to the open position
        if (isOpen && transform.position != openPosition)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, doorSpeed * Time.deltaTime);
        }
        // Else if the door is closed but has not completed closing, lerp to the close position
        else if (!isOpen && transform.position != closedPosition)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, doorSpeed * Time.deltaTime);
        }
    }
}
