using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : MonoBehaviour
{

    public GameObject Effects;

    public float Explosion_Radius;
    public float force;
    public float rcktLife;
    public float Rcktspeed;

    public int rcktDmg;

    [SerializeField] Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * Rcktspeed;
    }
    void OnTriggerEnter(Collider other)
    {
       
      BlowUp();
      Destroy(gameObject, rcktLife);
    }
    void BlowUp()
    {
      Collider[] colliders = Physics.OverlapSphere(transform.position, Explosion_Radius);
        foreach (Collider obj in colliders)
        {
           if (obj.CompareTag("Player"))
            {
                Instantiate(Effects, transform.position, transform.rotation);
            }
           var damage = obj.GetComponent<IDamage>();
           if (damage != null)
           {
              damage.takeDamage(rcktDmg);
           }
        }
    }
}
