using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    [SerializeField] Renderer model;
    Color colorOrig;
    
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.position != gameManager.instance.playerSpawnPos.transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(flashColor());
        }
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.magenta;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
