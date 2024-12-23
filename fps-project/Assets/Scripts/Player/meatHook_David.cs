using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using Unity.VisualScripting;
using UnityEngine;

public class meatHook : MonoBehaviour
{
    /// <summary>
    /// MeatHook is a grappling hook that latches onto objects with an "Enemy" or "ValidHookObject" Tag
    /// activate MeatHook with 'q'
    /// can cancel with 'q' or by jumping
    /// </summary>



    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private Transform meatHookTransform;
    [SerializeField] CharacterController controller;
    [SerializeField] float pullSpeed;
    [SerializeField] float pullRange;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip castSFX;
    [SerializeField] AudioClip pullSFX;
    [SerializeField] AudioClip breakSFX;

    Vector3 pullDestination;
    Vector3 pullDir;
    Vector3 momentum;

    RaycastHit hit;
    float chainLength;
    float castSpeed = 100f;

    private cameraFOV fovSlider;
    private float meatHookFOV;

    int origSpeed;
    int origGravity;
    private float origFov;

    bool isValidTarget;

    private State state;
    private enum State
    {
        Normal,
        CastingHook,
        Moving,
        Launched
    }



    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        fovSlider = Camera.main.GetComponent<cameraFOV>();

        origFov = Camera.main.fieldOfView;
        meatHookFOV = origFov * 2;

        origSpeed = gameManager.instance.player.GetComponent<playerController>().getSpeed();
        origGravity = gameManager.instance.player.GetComponent<playerController>().getGravity();

        // Setup audio
        if (audioSource != null && SFXControl.Instance != null)
        {
            SFXControl.Instance.SetupExistingAudioSource(audioSource);
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Normal:
                meatHook_Shoot();
                validTargetAssist();
                break;

            case State.CastingHook:
                castHook();
                break;

            case State.Moving:
                meatHook_Pull();
                break;

            case State.Launched:
                launchPlayer();
                meatHook_Shoot();
                validTargetAssist();
                break;

        }
    }



    private void meatHook_Shoot()
    {
        if (Input.GetButtonDown("Meat Hook"))
        {

            if (isValidTarget && Physics.Raycast(
                                    Camera.main.transform.position,
                                    Camera.main.transform.forward,
                                    out hit,
                                    pullRange,
                                    ~ignoreMask,
                                    QueryTriggerInteraction.Ignore))
            {


                hideHookIcon();


                //check if player is being pulled towards an enemy
                isDestinationAnEnemy(hit);


                pullDestination = hit.point;

                //scale and rotation
                resetHookAttributes();


                showHookModel();


                //start casting hook
                state = State.CastingHook;

                //play cast SFX
                audioSource.clip = castSFX;
                audioSource.Play();

                gameManager.instance.player.GetComponent<playerController>().isGrappling = true;
            }
        }
    }



    private void castHook()
    {
        //have hook rotate to look at destination
        meatHookTransform.LookAt(pullDestination);

        //increase chain length
        chainLength += castSpeed * Time.deltaTime;

        //apply new chain length
        meatHookTransform.localScale = new Vector3(0.2f, 0.2f, chainLength);

        //if reaches destination
        if (chainLength >= Vector3.Distance(transform.position, pullDestination))
        {


            //stop player wasd movement
            gameManager.instance.player.GetComponent<playerController>().haltMovement();

            //adjust fov of camera
            fovSlider.setFOV(meatHookFOV);



            //chain at location, now pull player
            state = State.Moving;

            //play pull SFX
            audioSource.clip = pullSFX;
            audioSource.Play();
        }
    }



    private void meatHook_Pull()
    {
        //direction to move in
        pullDir = (pullDestination - transform.position).normalized; //add a previous momentum for swing

        //move player
        controller.Move(pullDir * pullSpeed * Time.deltaTime);

        //update hook rotation
        meatHookTransform.LookAt(pullDestination);


        //scale chain back down to original size as you move forward
        //
        //decrease chain length
        chainLength -= pullSpeed * Time.deltaTime;
        //
        //some wierd bug where the chain somehow went negative, can't recreate it
        if (chainLength < 0f)
        {
            chainLength = 0;
        }
        //
        //apply new chain length
        meatHookTransform.localScale = new Vector3(0.2f, 0.2f, chainLength);
        //


        //have you reached destination -> turn off pull
        if (Vector3.Distance(controller.transform.position, pullDestination) <= 2f)
        {
            hideHookModel();

            gameManager.instance.player.GetComponent<playerController>().setSpeed(origSpeed);
            stopPull();

            state = State.Normal;

            gameManager.instance.player.GetComponent<playerController>().isGrappling = false;
        }
        //you can cancel Meat Hook by pressing 'q'/'jump'/or killing target
        else if (Input.GetButtonDown("Meat Hook") || Input.GetButtonDown("Jump") || hit.collider == null)
        {
            hideHookModel();

            //conserve your momentum (hook vector + jump vector)
            momentum = (pullDir * pullSpeed) + gameManager.instance.player.GetComponent<playerController>().getVelocity();

            stopPull();


            //play break SFX
            audioSource.clip = breakSFX;
            audioSource.Play();

            state = State.Launched;
        }
    }

    private void launchPlayer()
    {
        //check if on ground
        if (gameManager.instance.player.GetComponent<playerController>().isGroundedSpherecast())
        {
            state = State.Normal;
            momentum = Vector3.zero;
        }


        //momentum is lost over time, similar to jumping (but x/z shouldn't go below 0 else direction is reversed)
        //
        controller.Move(momentum * Time.deltaTime);//start moving

        if (momentum.magnitude >= 0f) //if over 0 magnitude
        {
            float drag = 1f;

            momentum -= momentum * drag * Time.deltaTime; //reduce momentum

            if (momentum.magnitude < 0.0f) //if you go under zero, set to zero
            {
                momentum = Vector3.zero;
            }
        }

        gameManager.instance.player.GetComponent<playerController>().isGrappling = false;
    }








    private void validTargetAssist()
    {
        RaycastHit hit;


        if (Physics.Raycast(
                Camera.main.transform.position, //where it starts
                Camera.main.transform.forward, //where it points
                out hit,
                pullRange, //how far out should it check for ground
                ~ignoreMask, //what layers to avoid
                QueryTriggerInteraction.Ignore))
        {

            //you hit something, check it's tag
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("ValidHookObject"))
            {
                showHookIcon();
                isValidTarget = true;
            }
            else
            {
                hideHookIcon();
                isValidTarget = false;
            }



        }
        else //hit nothing
        {
            hideHookIcon();
            isValidTarget = false;
        }
    }


    private void isDestinationAnEnemy(RaycastHit targetDestination)
    {
        //check if target uses 'EnemyAI' script and has an Enemy tag
        if (true == targetDestination.collider.gameObject.GetComponent<enemyAI>() && targetDestination.collider.CompareTag("Enemy"))
        {
            //Time = distance / speed
            float duration = Vector3.Distance(gameManager.instance.player.transform.position, pullDestination) / pullSpeed;

            //stun enemy for x amount of time, plus some extra padding time
            targetDestination.collider.gameObject.GetComponent<enemyAI>().stunEnemy(duration + 0.5f);
        }
    }


    private void stopPull()
    {
        //end SFX
        audioSource.Stop();
        audioSource.clip = null;


        gameManager.instance.player.GetComponent<playerController>().setJumpCount(1);
        fovSlider.setFOV(origFov);
        gameManager.instance.player.GetComponent<playerController>().setGravity(origGravity);
        gameManager.instance.player.GetComponent<playerController>().setSpeed(origSpeed);

        resetHookAttributes();
    }


    private void resetHookAttributes()
    {
        // reset hook scale
        meatHookTransform.localScale = Vector3.zero;
        // reset hook rotation
        meatHookTransform.transform.localRotation = Quaternion.identity;
        // reset chain size
        chainLength = 0f;
    }


    private void hideHookIcon()
    {
        gameManager.instance.meatHookIcon.SetActive(false);
    }
    private void showHookIcon()
    {
        gameManager.instance.meatHookIcon.SetActive(true);
    }


    private void hideHookModel()
    {
        meatHookTransform.gameObject.SetActive(false);
    }
    private void showHookModel()
    {
        meatHookTransform.gameObject.SetActive(true);
    }
}
