/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: Jacob Yates *
* *
* The interface that all interactable game objects should use to implement *
* interaction functionality. *
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // Should implement what happens when the object is interacted with
    void Interact(Interact interactor);
}
