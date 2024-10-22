using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript: MonoBehaviour
{
  
    public float rotateSpeed;

    private float startingH;
    private float timeOffset;
    // Start is called before the first frame update
     private void Start()
    {
        startingH = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
        
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += rotateSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x,rotation.y,rotation.z);

        
    }
}
