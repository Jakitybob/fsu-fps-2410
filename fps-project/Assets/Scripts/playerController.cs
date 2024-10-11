/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates * [Michael Bump] *
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

    //
    // MEMBER VARIABLES
    //

    // Movement related
    Vector3 moveDirection;
    Vector3 playerVelocity;
    int HPOrig;
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
        HPOrig = Hp;
        updatePlayerUI();
        interactor = GetComponent<Interact>();

    }

    // Update is called once per frame
    void Update()
    {
        // Perform checks every frame for movement and weapon use
        Move();
       
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
    /*
     * Implementation of the IDamage interface's takeDamage. When called,
     * lowers the player's health by the amount and checks to see if the player
     * has died or not.
     */
    public void takeDamage(int amount) 
    { 
        Hp -= amount;

        updatePlayerUI();
        StartCoroutine(damageFlash());

        if (Hp <= 0)
        {
            gameManager.instance.gameLost();
        }

    }

    public void updatePlayerUI()
    {
        gameManager.instance.PlayerHPBar.fillAmount = (float) Hp / HPOrig;
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDmgScreen.SetActive(false);
    }

    public void Heal(int amount)
    {
        Hp += amount;
        if (Hp > HPOrig)
        {
            Hp = HPOrig;
        }
        updatePlayerUI();

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
