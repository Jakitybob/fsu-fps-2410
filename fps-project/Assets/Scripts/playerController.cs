/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates * [NAME HERE] *
* *
* A brief description of the program should also be added here. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    //
    // SERIALIZED FIELDS
    //

    [SerializeField] CharacterController controller; // Player controller that handles moving the character and applying forces like gravity
    [SerializeField] LayerMask ignoreMask; // Layer mask used to tell the player what to ignore when raycasting

    // Attributes
    [SerializeField] int Hp;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpCountMax;
    [SerializeField] int gravity;

    // Weapon related data
    [SerializeField] int weaponDamage;
    [SerializeField] float fireRate;
    [SerializeField] int weaponRange;

    //
    // MEMBER VARIABLES
    //

    // Movement related
    Vector3 moveDirection;
    Vector3 playerVelocity;
    int jumpCount;
    bool isSprinting;
    bool isShooting;

    // Components
    private Interact interactor;

    //
    // FUNCTIONS
    //

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<Interact>();
    }

    // Update is called once per frame
    void Update()
    {
        // Perform checks every frame for movement and weapon use
        Move();
        FireWeapon();
        
        // Fire the interactor's raycast if the Interact key was pressed
        if (Input.GetButtonDown("Interact"))
        {
            interactor.InteractPressed();
        }
    }


    //
    // MOVEMENT FUNCTIONS
    //

    /*
     * This function, using the character controller, moves the player character based on
     * the input of the horizontal and vertical movement axes.
     */
    public void Move()
    {
        // Get the unit vector of the intended movement direction
        // TODO: Stop the player from moving faster while moving diagonally
        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        // Check if the player is jumping or not
        Jump();

        // Check if the player is sprinting or not
        Sprint();

        // Use the character controller to move the player based on speed and independent of framerate
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    /*
     * This function increases the player's speed by the sprint modifier amount if the sprint key is being pressed, otherwise
     * it returns the player's speed to normal.
     */
    public void Sprint()
    {
        // If the player pressed the sprint key, increase their speed
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        // Otherwise if the player released their sprint key, return their speed to normal
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    /*
     * This function makes the player's vertical velocity increase according to the jumpSpeed serialized field.
     * However, this jumpSpeed is also always exponentially decreased by the gravity serialized field.
     */
    public void Jump()
    {
        // If the player is on the ground reset the jump count to 0 and zero the playerVelocity vector so it doesn't get infinitely small
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        // If the jump input is pressed and the player hasn't exceeded the maximum number of jumps, increase the jump count and add the jump speed to the player's velocity
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }

        // Increase the player's velocity and move them in the corresponding direction
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    //
    // WEAPON RELATED FUNCTIONS
    //

    /*
     * This function starts the Shoot coroutine if the player is not already shooting and
     * they pressed or are holding the Fire1 input button.
     */
    public void FireWeapon()
    {
        // If the player is pressing fire and is not already shooting, start the shoot coroutine
        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    /*
     * This coroutine fires a raycast from the player's camera to the end of the weaponRange, and if a hit component
     * that can be damaged is found, it damages that object. No matter the case, this coroutine yields until the allotted
     * time of fireRate, in seconds, has passed, before allowing the player to shoot again.
     */
    private IEnumerator Shoot()
    {
        // Set the character to now shooting
        isShooting = true;

        // Fire a raycast from the camera out the range of the weapon and check for a result
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponRange, ~ignoreMask))
        {
            // If the collided component has the IDamage interface, damage the collided component
            IDamage damage = hit.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.takeDamage(weaponDamage);
            }
        }

        // Wait until the firerate has elapsed before allowing the player to shoot again
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    /*
     * Implementation of the IDamage interface's takeDamage. When called,
     * lowers the player's health by the amount and checks to see if the player
     * has died or not.
     */
    public void takeDamage(int amount) 
    { 
        Hp -= amount;
        if (Hp <= 0)
        {
            gameManager.instance.gameLost();
        }

    }


    //
    // GETTERS & SETTERS
    //

    // Returns the Interact component attached to the player
    public Interact GetInteractComponent()
    {
        return interactor;
    }
}
