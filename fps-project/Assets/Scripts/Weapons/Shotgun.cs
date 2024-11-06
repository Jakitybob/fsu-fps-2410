using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public int pellets;
    public float AngleofSpread;
    public float range;
    public int pelletdamage;
    public Transform ShootPos;



    void Update() 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();

        }
    }
    void shoot()
    {
        for (int i = 0; i < pellets; i++)
        {
            Quaternion spread = Quaternion.Euler(Random.Range(-AngleofSpread, AngleofSpread), Random.Range(-AngleofSpread, AngleofSpread), 0);
            Vector3 Dir = spread * ShootPos.forward;
            if (Physics.Raycast(ShootPos.position, Dir, out RaycastHit hit, range))
            {
                Debug.DrawRay(ShootPos.position, Dir * range, Color.red, 10.0f);
                if (hit.collider.CompareTag("Enemy"))
                {
                    var Damage = hit.collider.GetComponent<IDamage>();
                    if (Damage != null)
                    {
                        Damage.takeDamage(pelletdamage);
                        gameManager.instance.updateGameGoal(-1);
                    }
                }
            }
        }
    }
    
 }   