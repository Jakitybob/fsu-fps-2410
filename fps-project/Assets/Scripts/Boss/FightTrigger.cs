using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTrigger : MonoBehaviour
{

    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform spawnPoint;
    //
    //[SerializeField] List<GameObject> spawners
    //



    private void OnTriggerEnter(Collider other)
    {
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);

        //
        // turn on spawners here
        //

        //don't want trigger to stay and respawn bosses everytime you enter it
        Destroy(gameObject);
    }
}
