using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ResumeButton()
    {
        //gameManager.instance.unPause();
        //gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void RestartButton()
    {
        //gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RespawnPlayerButton()
    {
        //gameManager.instance.playerScript.resetPlayerHP();
        //gameManager.instance.unPause();
        //gameManager.instance.playerScript.setPlayerSpawnPoint();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void AddJumpButton()
    {
        //if (gameManager.instance.coins >= gameManager.instance.jumpCost)
        //{
        //    gameManager.instance.playerScript.addJump(amount);
        //}
    }
}
