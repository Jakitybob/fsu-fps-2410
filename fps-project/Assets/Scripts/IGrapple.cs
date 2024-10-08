using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrapple 
{
    void StartGrapple(Vector3 grappelPoint);

    void StopGrap();

    bool IsGrapple();

}
