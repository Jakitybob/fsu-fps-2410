using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
   
    public GameObject Effects;
    
    public float Explosion_Radius;
    public float force;
    public float rcktLife;
    public float Rcktspeed;
    
    public int rcktDmg;
   
    public LayerMask layer;
    
    private Rigidbody rb;




     void Start()
     {
        rb= GetComponent<Rigidbody>();
        rb.velocity= transform.forward * Rcktspeed;
     }

   void OnTriggerEnter(Collider other) 
    { 
        BlowUp();
        Destroy(gameObject, rcktLife); 
    }
    void BlowUp()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Explosion_Radius,layer);

        foreach (Collider obj in colliders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            Instantiate(Effects, transform.position, transform.rotation);

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
        Destroy(gameObject,rcktLife);

    }
}

