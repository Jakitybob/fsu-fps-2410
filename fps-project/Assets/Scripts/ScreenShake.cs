using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float decreaseFactor = 1.0f;

    //private Vector3 originalPos;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Shake()
    {
        
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);


            Vector3 offset = new Vector3(x, y, 0);
            transform.position = playerTransform.position + offset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        
        transform.position = playerTransform.position;

    }

}
