using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorScript : MonoBehaviour
{
    public delegate void DoorOpenedHandler();
    public event DoorOpenedHandler OnDoorOpened;

    // ... (other door logic)

    private void DoorOpened()
    {
        // ... (door opening logic)

        if (OnDoorOpened != null)
        {
            OnDoorOpened();
        }
    }
    
}
