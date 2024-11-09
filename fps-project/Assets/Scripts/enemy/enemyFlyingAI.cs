using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFlyingAI : MonoBehaviour, IDamage
{

    [SerializeField] GameObject explosionObject;
    [SerializeField] Renderer model;
    [SerializeField] Rigidbody rb;
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int detectionRange;
    [SerializeField] float attackDistance;
    [SerializeField] float explosionRadius;


    Vector3 playerDir;

    Color colorOrig;

    bool playerInRange;

    RaycastHit hit;

    GameObject objectToScale;


    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        GetComponent<SphereCollider>().radius = detectionRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            followPlayer();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;

        Debug.DrawRay(transform.position, playerDir);


        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void followPlayer()
    {
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;


        //check if in attack range -> explode
        if (hit.distance <= attackDistance)
        {
            explode();
            Destroy(gameObject);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashColor());


        if (HP <= 0)
        {
            explode();
            Destroy(gameObject);
        }
    }

    IEnumerator flashColor() //make enemy flash red
    {
        model.material.color = Color.red; //make red

        yield return new WaitForSeconds(0.1f); //wait 0.1 seconds

        model.material.color = colorOrig; //go back to original color
    }



    void explode()
    {
        objectToScale = Instantiate(explosionObject, transform.position, transform.rotation);
        objectToScale.transform.localScale *= explosionRadius;
    }
}
