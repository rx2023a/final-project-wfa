using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
