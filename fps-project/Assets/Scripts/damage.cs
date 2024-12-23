using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] enum damageType { bullet, stationary, rocket }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] float dotDelay;

    [SerializeField] GameObject explosionObject;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float explosionRadius;


    IDamage dmg;

    GameObject objectToScale;



    // Start is called before the first frame update
    void Start()
    {

        if (type == damageType.bullet || type == damageType.rocket)
        {
            rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
            Destroy(gameObject, destroyTime);
        }

        //play explosion effect
        if (explosionEffect != null)
        {
            objectToScale = Instantiate(explosionEffect, transform.position, transform.rotation);

            //explosion effect size is dependent on explosion AOE size
            //effect is half size of explosion AOE (looks better)
            objectToScale.transform.localScale = gameObject.transform.localScale / 2;
        }
    }

    private void Update()
    {
        //mute rockets on game pause
        if (Time.timeScale == 0)
        {
            if (GetComponentInChildren<AudioSource>() != null)
            {
                GetComponentInChildren<AudioSource>().enabled = false;
            }
        }
        else
        {
            if (GetComponentInChildren<AudioSource>() != null)
            {
                GetComponentInChildren<AudioSource>().enabled = true;
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger || other.CompareTag("Enemy") && type == damageType.stationary)
        {
            return;
        }


        dmg = other.GetComponent<IDamage>();


        if (dmg != null)
        {
            if (type == damageType.bullet || type == damageType.stationary)
            {
                dmg.takeDamage(damageAmount);
            }
        }


        if (type == damageType.rocket)
        {
            explode();
        }


        if (type == damageType.bullet || type == damageType.rocket)
        {
            Destroy(gameObject);
        }
        else if (type == damageType.stationary)
        {
            //do this method, delay of x seconds, repeat every x second
            //target is already damaged once on trigger enter, need the delay
            InvokeRepeating(nameof(repeatStationaryDamage), dotDelay, dotDelay);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (type == damageType.stationary)
        {
            CancelInvoke(nameof(repeatStationaryDamage));
        }
    }



    void explode()
    {
        objectToScale = Instantiate(explosionObject, transform.position, transform.rotation);
        objectToScale.transform.localScale *= explosionRadius;
    }



    void repeatStationaryDamage()
    {
        dmg.takeDamage(damageAmount);
    }
}
