using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunc : MonoBehaviour
{
    /*
     * Loads the first scene specified by the build index if in a build copy,
     * or simply loads LevelOne if in the editor.
     * 
     * TODO: Update this to work with saving and loading player data.
     */
    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne");
        
        // TODO: Modify this to work with saving and loading save data to go to the proper scene.
    }

    public void Resume()
    {
        gameManager.instance.UnPaused();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.UnPaused();
    }

    public void Respawn()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.UnPaused();
    }
    public void Quit() 
    {
        #if UNITY_EDITOR
            if ("MainMenu" != SceneManager.GetActiveScene().name)
            {
                gameManager.instance.UnPaused();
                gameManager.instance.changeLevel("MainMenu");
                //SceneManager.LoadScene("MainMenu");
            }
            else
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }

#else
            if ("MainMenu" != SceneManager.GetActiveScene().name)
            {
                gameManager.instance.UnPaused();
                gameManager.instance.changeLevel("MainMenu");
                //SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Application.Quit();
            }
#endif
    }

    public void showcase()
    {
        SceneManager.LoadScene("Showcase");
    }
    
}
