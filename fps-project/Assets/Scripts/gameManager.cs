using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;

    public bool isPaused;

    float OrigTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        OrigTime = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                Paused();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause) {
                UnPaused();
              
            }
        }
    }
    public void Paused()
    {
        isPaused=!isPaused;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void UnPaused()
    {
        isPaused= !isPaused;
        Time.timeScale = OrigTime;
        Cursor.visible= false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive= null;
        

    }
}
