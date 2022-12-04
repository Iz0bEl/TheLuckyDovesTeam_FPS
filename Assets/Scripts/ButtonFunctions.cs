using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ResumeButton()
    {

    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RespawnPlayerButton()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void AddJumpButton()
    {

    }
}
