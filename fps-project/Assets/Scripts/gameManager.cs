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
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public GameObject meatHookIcon;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;

    public playerController playerScript;
    public GameObject playerSpawnPos; 

    public GameObject player;
    [SerializeField] TMP_Text enemyText;

    //declare private variable
    public Image playerHPBar;
    public GameObject playerDmgScreen;
    [SerializeField] TMP_Text currentAmmoText;
    [SerializeField] TMP_Text totalAmmoText;

    public bool isPaused;

    float OrigTime;
    int enemycount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        OrigTime = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        playerScript = player.GetComponent<playerController>();
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
    }
    public void gameLost()
    {
        Paused();
        menuActive = Lose;
        menuActive.SetActive(true);
    }
    public void gameWon()
    {
        Paused();
        menuActive = Win;
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

    //
    // GETTERS & SETTERS
    //

    public Image PlayerHPBar
    {
        get { return playerHPBar; }
        set { playerHPBar = value; }
    }

    public TMP_Text GetCurrentAmmoText() { return currentAmmoText; }
    public TMP_Text GetTotalAmmoText() { return totalAmmoText; }




    public void changeLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }


}
