/************************************************************************************ 
* * Full Sail GDB229 FPS Project *
* Developers: [Gyoed Crespo] * [Michael Bump] * [David Oross]
* *
* This is the game manager so all things that the game needs to see/use for the game to run. *
************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;

    public GameObject player;
    [SerializeField] TMP_Text enemyText;

    //declare private variable
    public Image playerHPBar;
    public GameObject playerDmgScreen;

    public bool isPaused;

    float OrigTime;
    int enemycount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        OrigTime = Time.timeScale;
        player = GameObject.FindWithTag("Player");
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
            else if (menuActive == menuPause) 
            {
                UnPaused();          
            }
        }

        if (Input.GetButtonDown("Inventory"))
        {
            if (menuActive == null)
            {
                openInventory();
                menuActive = menuInventory;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuInventory)
            {
                closeInventory();
            }
        }
    }
    public void Paused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void UnPaused()
    {
        isPaused = !isPaused;
        Time.timeScale = OrigTime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void updateGameGoal(int amount)
    {
        enemycount += amount;
        enemyText.text = enemycount.ToString("F0");
        if(enemycount<= 0)
        {
            Paused();
            menuActive = Win;
            menuActive.SetActive(true);
        }
    }
    public void gameLost()
    {
        Paused();
        menuActive = Lose;
        menuActive.SetActive(true);
    }
    public void openInventory()
    {
        isPaused = !isPaused;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void closeInventory()
    {
        isPaused = !isPaused;
        Time.timeScale = OrigTime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
    public Image PlayerHPBar
    {
        get { return playerHPBar;}
        set {playerHPBar = value;}
    } 
}
