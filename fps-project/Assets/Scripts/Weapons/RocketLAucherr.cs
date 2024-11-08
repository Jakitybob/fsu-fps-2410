using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketlaucherr : MonoBehaviour
{
    public GameObject rckt;
    public Transform ShootPos;
    public float Force;
    public float Rate;
    public float delay;


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            FireRocket();
            delay = Time.deltaTime + 1f;
        }


    }
    void FireRocket()
    {
        GameObject Rocket = Instantiate(rckt, ShootPos.position, ShootPos.rotation);
        Rigidbody rb = Rocket.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(ShootPos.forward * Force);

        } 


    }

}
