using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunc : MonoBehaviour
{
    public void Resume()
    {
        gameManager.instance.UnPaused();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.UnPaused();
    }
    public void Quit()
    {
      #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
      #else
        Application.Quit();
     #endif
    }
}
