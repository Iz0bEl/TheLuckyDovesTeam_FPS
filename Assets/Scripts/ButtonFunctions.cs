using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ResumeButton()
    {
        GameManager.instance.unPause();
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
    }

    public void RestartButton()
    {
        GameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RespawnPlayerButton()
    {
        GameManager.instance.playerScript.resetPlayerHP();
        GameManager.instance.unPause();
        GameManager.instance.playerScript.setPlayerSpawnPoint();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void AddJumpButton(int amount)
    {
        if (GameManager.instance.coins >= GameManager.instance.jumpCost)
        {
           GameManager.instance.playerScript.addJump(amount);
        }
    }

    public void AutoRifleButton()
    {
       //GameManager.instance.playerScript.;
    }

    public void ShotgunButton()
    {

    }

    public void SniperButton()
    {

    }
}
