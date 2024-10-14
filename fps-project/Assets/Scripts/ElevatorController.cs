using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private float startHeight = 0f;
    [SerializeField] private float endHeight = 10f;
    [SerializeField] private float speed = 2f;
    

    private bool isGoingUp = true;
    private bool isMoving = false;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    private void Start()
    {
        targetPosition = transform.position + Vector3.up * startHeight;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position,
 targetPosition) < 0.1f)
            {
                isMoving
 = false;

                // Determine the next target position based on the current direction
                if (isGoingUp)
                {
                    targetPosition = transform.position + Vector3.up * (endHeight - startHeight);
                }
                else
                {
                    targetPosition = transform.position - Vector3.up * (endHeight - startHeight);
                }
            }
        }
    }

    // Implementation of the IInteractable interface
    public void Interact(Interact interactor)
    {
        if (!isMoving)
        {
            isGoingUp = !isGoingUp; // Toggle the direction on each interaction
            isMoving = true; // Start the elevator's movement
        }
    }
}
    
