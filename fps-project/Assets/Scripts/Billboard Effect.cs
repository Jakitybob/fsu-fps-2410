using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    [SerializeField] Transform headPos;

    Vector3 playerDir;

    int rotationSpeed = 100;



    // Update is called once per frame
    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        facePlayer();
    }



    void facePlayer()
    {
        // save rotation need to look at player (ignore y-level)
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));

        // smoothly spin object
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }
}
