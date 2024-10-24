using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour, IDamage
{

    [SerializeField] GameObject boss;
    [SerializeField] Renderer model;

    Color colorOrig;



    void Start()
    {
        colorOrig = model.material.color;
    }



    public void takeDamage(int amount)
    {
        if (boss.GetComponent<BossAI>())
        {
            boss.GetComponent<BossAI>().takeDamage(amount);

            StartCoroutine(flashHit());
        }
    }



    IEnumerator flashHit()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        model.material.color = colorOrig;
    }
}
