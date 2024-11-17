using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosiontrigger : MonoBehaviour
{
    public AudioClip explosionSound;
    public ScreenShake screenShake;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play explosion sound
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

            // Trigger screen shake
            screenShake.Shake();

            Destroy(gameObject);
        }
    }
}
