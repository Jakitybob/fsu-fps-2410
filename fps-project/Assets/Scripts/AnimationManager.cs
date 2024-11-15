using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    [SerializeField] Animator punchAnim;






    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    
    public void playPunchAnim()
    {
        punchAnim.SetTrigger("Punch");
    }
}
