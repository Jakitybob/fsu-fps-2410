/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: [David Oross] * [Michael Bump] * [Jacob Yates (Simple Bugfixing)] *
* *
* This where all things enemy related are. . *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class enemyAI : MonoBehaviour, IDamage, IInteractable
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject weapon;
    [SerializeField] List<GameObject> gloryKillDrops;
    [SerializeField] int gloryKillDiceSize;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioMixerGroup sfxGroup; // Reference to store the SFX group

    [SerializeField] float attackRate;
    [SerializeField] float detectionRange;
    [SerializeField] float walkSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] int viewAngle;

    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    [SerializeField] int HP;

    int origHP;

    bool isShooting;
    bool playerInRange;
    bool isRoaming;
    bool isWalkingAudio;

    float angleToPlayer;
    float stoppingDistOrig;

    bool isShowingExecuteFlash;
    bool isExecutable;
    bool isStunned;

    Vector3 playerDir;
    Vector3 startingPos;

    Color colorOrig;

    Coroutine someCo;

    Coroutine damageFlashCo;

    void Awake()
    {
        // Setup audio source if not already setup
        SetupAudioSource();
    }

    // Start is called before the first frame update
    void Start()
    {
        origHP = HP;
        gameManager.instance.updateGameGoal(1);
        colorOrig = model.material.color;
        GetComponent<SphereCollider>().radius = detectionRange;
        agent.GetComponent<NavMeshAgent>().speed = walkSpeed;
        
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        // Double-check audio setup
        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        // If we don't have an audio source, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Try to get the SFX group from the mixer manager
        if (audioSource.outputAudioMixerGroup == null)
        {
            if (MixerManager.Instance != null)
            {
                sfxGroup = MixerManager.Instance.GetSFXGroup();
                if (sfxGroup != null)
                {
                    audioSource.outputAudioMixerGroup = sfxGroup;
                }
            }
        }

        // Configure audio source for better 3D sound
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;  // Full 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;  // Better distance falloff
        audioSource.minDistance = 1f;  // Start falloff at 1 unit
        audioSource.maxDistance = detectionRange;  // Stop at detection range
        audioSource.spread = 60f;  // Wider sound spread
        audioSource.dopplerLevel = 0f;  // Disable doppler effect
        audioSource.reverbZoneMix = 1f;  // Full reverb mix
        audioSource.priority = 128;  // Default priority
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        facePlayer();

        ////play walking audio
        //if (agent.velocity.normalized.magnitude > 0 && walkSFX != null && !isWalkingAudio)
        //{
        //    StartCoroutine(walkNoise());
        //}
        //else if (agent.velocity.normalized.magnitude == 0 && walkSFX != null &&walkingAudioSource.isPlaying)
        //{
        //    isWalkingAudio = false;
        //    walkingAudioSource.Stop();
        //}


        //when enemy is low health, start flashing yellow, prevent all other actions
        if (true == isExecutable)
        {
            //freeze movement
            agent.GetComponent<NavMeshAgent>().speed = 0;

            if (false == isShowingExecuteFlash)
            {
                StartCoroutine(flashExecution());
            }
            return;
        }


        

        if (playerInRange)
        {
            if (!canSeePlayer() && !isRoaming && agent.remainingDistance < 0.01f)
            {
                someCo = StartCoroutine(roam());
            }
        }
        else if (!playerInRange)
        {
            if (!isRoaming && agent.remainingDistance < 0.01f)
            {
                someCo = StartCoroutine(roam());
            }
        }
    }

    //IEnumerator walkNoise()
    //{
    //    isWalkingAudio = true;

    //    walkingAudioSource.clip = walkSFX;
    //    walkingAudioSource.Play();

    //    yield return new WaitForSeconds(walkingAudioSource.clip.length);

    //    isWalkingAudio = false;
    //}


    IEnumerator roam()
    {
        isRoaming = true;

        yield return new WaitForSeconds(roamPauseTime);

        agent.stoppingDistance = 0;

        Vector3 randomDist = Random.insideUnitSphere * roamDist;

        randomDist += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDist, out hit, roamDist, 1);
        agent.SetDestination(hit.position);

        isRoaming = false;
        someCo = null;
    }



    bool canSeePlayer()
    {
        //angleToPlayer = Vector3.Angle(playerDir,transform.forward); // Commented out at the moment as this doesn't apply since the enemies always face the player
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                
                if (playerInRange && !isStunned)
                {
                    //StartCoroutine(flashExecution()); // Commented out since it's not implemented fully yet 

                    if (!isShooting)
                    {
                        StartCoroutine(shoot());
                    }
                }

                //if player seen, turn on original stopping dist
                agent.stoppingDistance = stoppingDistOrig;

                return true;
            }
        }

        //if player not seen, set stopping dist to 0
        agent.stoppingDistance = 0;

        return false;
    }


    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }


    IEnumerator shoot()
    {
        isShooting = true;

        animator.SetTrigger("Shoot");

        Instantiate(weapon, shootPos.position, transform.rotation);

        if (shootSFX != null && audioSource != null)
        {
            audioSource.clip = shootSFX;
            audioSource.Play();
        }

        yield return new WaitForSeconds(attackRate);

        isShooting = false;
    }


    public void takeDamage(int amount)
    {
        
        HP -= amount;

        if (!isExecutable)
        {
            damageFlashCo = StartCoroutine(flashHit());
        }

        if (hurtSFX != null && audioSource != null)
        {
            audioSource.clip = hurtSFX;
            audioSource.Play();
        }

        //if roaming, stop
        if (someCo != null)
        {
            StopCoroutine(someCo);
            isRoaming = false;
        }


        if (HP <= 0)
        {

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            //if (walkSFX != null)
            //{
            //    walkingAudioSource.Stop();
            //    walkingAudioSource = null;
            //}

            //execution state health
            HP = origHP;


            //roll for glory kill
            int gloryKillRoll = Random.Range(0, gloryKillDiceSize + 1); //if (0,4) it will only roll 1-3

            //if already in execution state, kill
            if (gloryKillRoll != 0 || true == isExecutable)
            {
                //no glory kill
                gameManager.instance.updateGameGoal(-1);
                Destroy(gameObject);
            }
            else
            {
                //can glory kill
                StopCoroutine(damageFlashCo);
                model.material.color = colorOrig;
                isExecutable = true;
            }                
        }
    }


    public void stunEnemy(float duration)
    {
        StartCoroutine(stunDuration(duration));
    }
    IEnumerator stunDuration(float duration)
    {
        //freeze movement
        agent.GetComponent<NavMeshAgent>().speed = 0;
        //stop from shooting
        isStunned = true;

        yield return new WaitForSeconds(duration);

        //unfreeze movement
        agent.GetComponent<NavMeshAgent>().speed = walkSpeed;
        //allow shooting
        isStunned = false;
    }


    IEnumerator flashHit()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.01f);
        model.material.color = colorOrig;
    }


    IEnumerator flashExecution()
    {
        isShowingExecuteFlash = true;

        model.material.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);
        model.material.color = colorOrig;
        yield return new WaitForSeconds(0.5f);

        isShowingExecuteFlash = false;
    }

    public void Interact(Interact interactor)
    {
        if (isExecutable == true)
        {
            //roll 2 sided dice
            int dropRoll = Random.Range(0, gloryKillDrops.Count);


            //instantiate object in gloryKillDrops at index [dropRoll]
            Vector3 dropPos = transform.position + new Vector3(0, 1, 0);
            Instantiate(gloryKillDrops[dropRoll], dropPos, transform.rotation);



            //play melee animation on player
            AnimationManager.instance.playPunchAnim();
            interactor.GetComponent<playerController>().playPunchAudio();


            //destroy enemy object
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    public void setDetectionRange(float newRange)
    {
        detectionRange = newRange;
    }

    public void setRoamingRange(int newRoamingRange)
    {
        roamDist = newRoamingRange;
    }
}
