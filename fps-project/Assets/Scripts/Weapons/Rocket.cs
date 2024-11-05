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
    public float Rcktspeed;
    private Rigidbody rb;




     void Start()
     {
        rb= GetComponent<Rigidbody>();
        rb.velocity= transform.forward * Rcktspeed;
        Invoke(nameof(BlowUp), rcktLife); 
     }

   /* void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Boom" + collision.gameObject.name);
        Debug.Log("@" + collision.contacts[0].point);
        BlowUp();
        Destroy(gameObject);

    }*/
    void BlowUp()
    {
        Instantiate(Effects, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, Explosion_Radius);

        foreach (Collider obj in colliders)
        {
            
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, Explosion_Radius);
                }
                var damage = obj.GetComponent<IDamage>();
                if (damage != null)
                { 
                    damage.takeDamage(rcktDmg); 
                }
            
        }
        Destroy(gameObject);

    }
}

