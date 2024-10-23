using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] enum damageType { bullet, stationary }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] float dotDelay;


    IDamage dmg;



    // Start is called before the first frame update
    void Start()
    {
        if (type == damageType.bullet)
        {
            //rb.velocity = transform.forward * speed;
            rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
            Destroy(gameObject, destroyTime);
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
            dmg.takeDamage(damageAmount);
        }

        if (type == damageType.bullet)
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



    // Update is called once per frame
    void Update()
    {
        
    }



    void repeatStationaryDamage()
    {
        dmg.takeDamage(damageAmount);
    }
}
