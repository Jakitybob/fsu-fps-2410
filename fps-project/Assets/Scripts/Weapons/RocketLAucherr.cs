using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketlaucherr : MonoBehaviour
{
    public GameObject rckt;
    public Transform ShootPos;
    public float Force;
    public float Rate;
    public int maxAmmo;
    public float reloadtime;

    private int ammoCount;
 
    private bool isReload = false;


    // Update is called once per frame
    private void Start()
    {
        ammoCount = maxAmmo;
    }
    void Update()
    {
        //if (isReload)
           // return;
        gameManager.instance.GetCurrentAmmoText();
        gameManager.instance.GetTotalAmmoText();
        if (Input.GetButtonDown("Fire1")&& ammoCount > 0)
        {
            FireRocket();
        }
        if(Input.GetKeyDown(KeyCode.R)&& ammoCount <= 0 && !isReload)
        {
            StartCoroutine(Reload());
        }


    }
    void FireRocket()
    {
        ammoCount--;
        GameObject Rocket = Instantiate(rckt, ShootPos.position, ShootPos.rotation);
        Rigidbody rb = Rocket.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(ShootPos.forward * Force);

        }

    }
   
    IEnumerator Reload()
    {
        isReload = true;
        yield return new WaitForSeconds(reloadtime);
        ammoCount=maxAmmo;
        isReload = false;
    }

}
