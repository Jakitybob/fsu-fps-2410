using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTeleporter : MonoBehaviour
{

    [SerializeField] GameObject exitLocation;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = exitLocation.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
