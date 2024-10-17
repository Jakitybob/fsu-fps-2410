using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawerScript : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject []Enemy;
    [SerializeField] Transform[] spawnpoint;
    [SerializeField] float spawntimer;
    [SerializeField] int maxSpawn;

    int count;
    private void Start()
    {
        StartCoroutine(SpawnObject());
        
    }
    IEnumerator SpawnObject()
    {
        while (count < maxSpawn) { 
           yield return new WaitForSeconds(spawntimer);
           
           Spawn();

        }
    }
    void Spawn()
    {
        int randomSpawnIndex = Random.Range(0, spawnpoint.Length);
       
        Transform spawnPoint = spawnpoint[randomSpawnIndex];
        
        int randomObjectIndex = Random.Range(0, Enemy.Length);
        
        GameObject objectToSpawn = Enemy[randomObjectIndex];
       
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        
        count++;
    }

}
