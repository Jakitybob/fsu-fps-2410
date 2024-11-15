/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates * [Michael Bump] * [Z Broyles]
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

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip punchSFX;

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
    int defaultLayer;
    float distToGround = 0.1f;
    float spherecastRadius;

    // Components
    private Interact interactor;
    private PlayerWeaponComponent weaponComponent;
    

    //
    // FUNCTIONS
    //

    // Start is called before the first frame update

   

    void Start()
    {
        // Set all baseline values and get all components
        HPOrig = Hp;
        interactor = GetComponent<Interact>();
        weaponComponent = GetComponent<PlayerWeaponComponent>();

        // Update the UI
        updatePlayerUI();

        spherecastRadius = transform.localScale.x / 2;
        distToGround += transform.localScale.y / 2;


        defaultLayer = LayerMask.GetMask("Default");

        spawnPlayer();
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        Hp = HPOrig;
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

        // Call attack with the current weapon if the Fire1 key is being pressed
        if (Input.GetButton("Fire1"))
        {
            weaponComponent.WeaponAttack();
            updatePlayerUI();
        }

        // Call reload on the current weapon if the Reload key is pressed down and the user is not firing
        if (Input.GetButtonDown("Reload") && !Input.GetButton("Fire1"))
        {
            weaponComponent.Reload();
            updatePlayerUI();
        }

        // Check for switching weapons
        SwitchWeapons();
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
        if (isGroundedSpherecast())
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
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    // Method to apply KnockBack physics without RigidBody
    public void KnockBack(Vector3 dir , float force)
    {
        transform.position += dir*force *Time.deltaTime;

    }
    /*
     * Performs checks for mouse wheel input and switches the user's weapons
     * accordingly. All range checks are performed within the weapon component
     * and only input is checked here.
     * 
     * TODO: Implement 1->2->3->...N number keys for switching as well
     */
    void SwitchWeapons()
    {
        // NOTE: Range checks are done in the weapon component so no need here
        // If scrolling up, increment the weapon index and update the weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponComponent.GetWeaponListCount() > 0)
        {
            weaponComponent.SetWeaponIndex(weaponComponent.GetWeaponIndex() + 1);
            weaponComponent.SetWeaponStats(weaponComponent.GetCurrentWeapon());
            updatePlayerUI();
        }
        // Otherwise if scrolling down, decrement the weapon index and update the weapon
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponComponent.GetWeaponListCount() > 0)
        {
            weaponComponent.SetWeaponIndex(weaponComponent.GetWeaponIndex() - 1);
            weaponComponent.SetWeaponStats(weaponComponent.GetCurrentWeapon());
            updatePlayerUI();
        }
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

    /*
     * Update any player-facing GUI elements:
     * > Health
     * > Current and Total Ammo for Weapon
     */
    public void updatePlayerUI()
    {
        gameManager.instance.PlayerHPBar.fillAmount = (float) Hp / HPOrig;

        // If there is a weapon, update the ammo
        if (weaponComponent.GetWeaponListCount() > 0)
        {
            gameManager.instance.GetCurrentAmmoText().text = weaponComponent.GetCurrentAmmo().ToString();
            gameManager.instance.GetTotalAmmoText().text = weaponComponent.GetTotalAmmo().ToString();
        }
        
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



    //custom isGrounded method that uses raycast to detect objects with "Default" tag
    public bool isGroundedSpherecast()
    {
        bool isGrounded = false;


        RaycastHit hit;

        if (Physics.SphereCast(
                                transform.position,
                                spherecastRadius,
                                Vector3.down,
                                out hit,
                                distToGround,
                                defaultLayer,
                                QueryTriggerInteraction.Ignore)
        )
        {
            isGrounded = true;
        }


        return isGrounded;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * distToGround, spherecastRadius);
    }


    public void playPunchAudio()
    {
        audioSource.clip = punchSFX;
        audioSource.Play();
    }


    //
    // GETTERS & SETTERS
    //

    // Returns the Interact component attached to the player
    public Interact GetInteractComponent() { return interactor; }

    // Returns the weapon component attached to the player
    public PlayerWeaponComponent GetWeaponComponent() { return weaponComponent; }


    public void setSpeed(int newSpeed)
    {
        speed = newSpeed;
    }
    public int getSpeed()
    {
        return speed;
    }


    public void setGravity(int newGravity)
    {
        gravity = newGravity;
    }
    public int getGravity()
    {
        return gravity;
    }


    public void setVelocity(Vector3 newVelocity)
    {
        playerVelocity = newVelocity;
    }
    public Vector3 getVelocity()
    {
        return playerVelocity;
    }


    public void setJumpCount(int numOfJumps)
    {
        jumpCount = numOfJumps;
    }


    public void haltMovement()
    {
        setSpeed(0);
        setGravity(0);

        playerVelocity = Vector3.zero;
    }
}
