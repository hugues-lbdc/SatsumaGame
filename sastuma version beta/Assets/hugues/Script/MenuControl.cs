using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuControl : MonoBehaviour
{
  public void ButtonPlay()
    {
        SceneManager.LoadScene(3);
    }

    public void ButtonOnline()
    {
        SceneManager.LoadScene(2);
    }
    public void ButtonStory()
    {
        SceneManager.LoadScene(3);
    }




    public void ButtonQuit()
    {
        Debug.Log("Quit !");
        Application.Quit();
    }
}
