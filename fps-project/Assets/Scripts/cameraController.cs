/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates * [NAME HERE] *
* *
* A brief description of the program should also be added here. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    //
    // SERIALIZED FIELDS
    //

    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    //
    // MEMBER VARIABLES
    //

    float rotationX;


    //
    // FUNCTIONS
    //

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input from the x and y axes of the mouse
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        // Handle camera inversion
        if (!invertY)
        {
            rotationX -= mouseY;
        }
        else
        {
            rotationX += mouseY;
        }

        // Clamp the rotationX on the x-axis within specified parameters
        rotationX = Mathf.Clamp(rotationX, lockVertMin, lockVertMax);

        // Rotate the camera about the x-axis (up and down)
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Turn the player about the y-axis (left and right)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
