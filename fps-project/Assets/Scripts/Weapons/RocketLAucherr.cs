using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketlaucherr : MonoBehaviour
{
    public GameObject rckt;
    public Transform ShootPos;
    public Transform LaucherrPos;
    public float Force;
    public float Rate;
    public int maxAmmo;
    public float reloadtime;
   

    private int ammoCount;
 
    private bool isReload = false;
    private bool pickerUP = false;


    // Update is called once per frame
    private void Start()
    {
        ammoCount = maxAmmo;
    }
    void Update()
    {
        if (pickerUP)
        {
            gameManager.instance.GetCurrentAmmoText();
            gameManager.instance.GetTotalAmmoText();
            if (Input.GetButtonDown("Fire1") && ammoCount > 0)
            {
                FireRocket();
            }
            if (Input.GetKeyDown(KeyCode.R) && ammoCount <= 0 && !isReload)
            {
                StartCoroutine(Reload());
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&& !pickerUP)
        {
            pickUpLauncher(other.transform);
        }
        
    }
    void pickUpLauncher(Transform player)
    {
        pickerUP = true;
        transform.SetParent(LaucherrPos);
        transform.localPosition=Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;


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
