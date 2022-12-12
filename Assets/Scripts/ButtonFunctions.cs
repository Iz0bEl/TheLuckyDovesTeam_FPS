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

    public void AddSpeedButton(int amount)
    {
        if (GameManager.instance.coins >= GameManager.instance.speedCost)
        {
            GameManager.instance.playerScript.addSpeed(amount);
        }
    }

    //public void AutoRifleButton()
    //{
    //    GameManager.instance.playerScript.rifleEquiped = true;
    //    GameManager.instance.playerScript.shotgunEquiped = false;
    //    GameManager.instance.playerScript.sniperEquiped = false;
    //    GameManager.instance.SniperScopeUI.SetActive(false);
    //    Camera.main.fieldOfView = 60;
    //    GameManager.instance.unPause();
    //}

    //public void ShotgunButton()
    //{
    //    GameManager.instance.playerScript.rifleEquiped = false;
    //    GameManager.instance.playerScript.shotgunEquiped = true;
    //    GameManager.instance.playerScript.sniperEquiped = false;
    //    GameManager.instance.SniperScopeUI.SetActive(false);
    //    Camera.main.fieldOfView = 60;
    //    GameManager.instance.unPause();
    //}

    //public void SniperButton()
    //{
    //    GameManager.instance.playerScript.rifleEquiped = false;
    //    GameManager.instance.playerScript.shotgunEquiped = false;
    //    GameManager.instance.playerScript.sniperEquiped = true;
    //    GameManager.instance.unPause();
    //}
}
