using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTrigger : MonoBehaviour
{

    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform spawnPoint;

    [SerializeField] GameObject door;
    [SerializeField] Transform doorPos;



    private void OnTriggerEnter(Collider other)
    {
        //spawn boss
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.localRotation);

        //spawn locked door
        Instantiate(door, doorPos.transform.position, doorPos.transform.localRotation);


        //don't want trigger to stay and respawn bosses everytime you enter it
        Destroy(gameObject);
    }
}
