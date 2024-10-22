using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour, IDamage
{

    [SerializeField] GameObject boss;


    public void takeDamage(int amount)
    {
        if (boss.GetComponent<BossAI>())
        {
            boss.GetComponent<BossAI>().takeDamage(amount);
        }
    }
}
