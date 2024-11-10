using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenuActive;
    [SerializeField] GameObject menuStarting;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuCreditsPanel;

    bool inSettings;
    bool inCredits;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuActive = menuStarting;
        inSettings = false;
        inCredits = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            selectStartingButton();
        }
    }


    public void selectStartingButton()
    {
        //first button should always be first child of menu
        EventSystem.current.SetSelectedGameObject(mainMenuActive.transform.GetChild(0).gameObject);
    }


    public void swapMenu()
    {
        inSettings = !inSettings;

        if (true == inSettings)
        {
            mainMenuActive = menuSettings;
        }
        else
        {
            mainMenuActive = menuStarting;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }


    public void toggleCreditsPanel()
    {
        inCredits = !inCredits;

        if (true == inCredits)
        {
            menuCreditsPanel.SetActive(true);
        }
        else
        {
            menuCreditsPanel.SetActive(false);
        }
    }
}
