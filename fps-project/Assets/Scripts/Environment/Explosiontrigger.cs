using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosiontrigger : MonoBehaviour
{
    [SerializeField] AudioClip explosionSound;
    [SerializeField] float explosionVolume = 1f;
    public ScreenShake screenShake;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play explosion sound through SFXControl
            if (explosionSound != null && SFXControl.Instance != null)
            {
                SFXControl.Instance.playSound(explosionSound, transform, explosionVolume);
            }

            // Trigger screen shake
            screenShake.Shake();

            Destroy(gameObject);

            
        }
    }
    
}
