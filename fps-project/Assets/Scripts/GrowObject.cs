using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowObject : MonoBehaviour
{
    public float growthRate = 0.1f; // Adjust this value to control the growth speed
    public float targetHeight = 90f; // The final height of the object

    private Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(initialScale, new Vector3(initialScale.x, targetHeight, initialScale.z), growthRate * Time.deltaTime);
    }
}
