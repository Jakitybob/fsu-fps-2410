using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwap_Trigger : MonoBehaviour
{

    [SerializeField] string nextSceneName;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.changeLevel(nextSceneName);
        }
    }




}
