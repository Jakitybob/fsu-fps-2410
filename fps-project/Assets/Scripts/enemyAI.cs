/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: [David Oross] * [Michael Bump] * [Jacob Yates (Simple Bugfixing)] *
* *
* This where all things enemy related are. . *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject weapon;
    [SerializeField] float attackRate;
    [SerializeField] float detectionRange;
    [SerializeField] float walkSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] int viewAngle;

    [SerializeField] int HP;

    bool isShooting;
    bool playerInRange;

    float angleToPlayer;

    bool showExecuteFlash;
    bool isStunned;


    Vector3 playerDir;

    Color colorOrig;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        colorOrig = model.material.color;
        GetComponent<SphereCollider>().radius = detectionRange;
        agent.GetComponent<NavMeshAgent>().speed = walkSpeed;        
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        facePlayer();

        if (playerInRange)
        {
            canSeePlayer();
        }

        //when enemy is low health, start flashing yellow
        if (HP == 1 && !showExecuteFlash)
        {
            StartCoroutine(flashExecution());
        }
    }

    bool canSeePlayer()
    {
        //angleToPlayer = Vector3.Angle(playerDir,transform.forward); // Commented out at the moment as this doesn't apply since the enemies always face the player
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.Log(hit.collider.name);
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
                return true;
            }
        }
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
        }
    }


    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(weapon, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(attackRate);


        isShooting = false;
    }


    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashHit());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        else if (HP == 1)
        {
            isStunned = true;
            agent.GetComponent<NavMeshAgent>().speed = 0;
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
        showExecuteFlash = true;

        model.material.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);
        model.material.color = colorOrig;
        yield return new WaitForSeconds(0.5f);

        showExecuteFlash = false;
    }

}
