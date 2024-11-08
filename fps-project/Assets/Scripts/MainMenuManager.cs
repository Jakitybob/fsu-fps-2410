using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenuActive;
    [SerializeField] GameObject menuStarting;
    [SerializeField] GameObject menuSettings;

    bool inSettings;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuActive = menuStarting;
        inSettings = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            selectStartingButton();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (mainMenuActive == menuSettings)
            {
                mainMenuActive = menuStarting;
                menuSettings.gameObject.SetActive(false);
                menuStarting.gameObject.SetActive(true);

                selectStartingButton();
            }
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
}
