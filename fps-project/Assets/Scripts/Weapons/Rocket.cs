using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
   
    public GameObject Effects;
    public float Explosion_Radius;
    public float force;
    public float rcktLife;
    public int rcktDmg;


    void Update()
    {
        Destroy(gameObject, rcktLife);
    }
    void OnCollisionEnter(Collision collision)
    {
        BlowUp();
        Destroy(gameObject);

    }
    void BlowUp()
    {
        Instantiate(Effects, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, Explosion_Radius);

        foreach (Collider obj in colliders)
        {
            if (obj.CompareTag("Enemy"))
            {

                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, Explosion_Radius);
                }
                var enemy = obj.GetComponent<IDamage>();
                if (enemy != null)
                { 
                    enemy.takeDamage(rcktDmg); 
                }
            }
        }   

    }
}

