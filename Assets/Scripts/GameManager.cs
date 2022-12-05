using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("------Player Stuff------")]
    public GameObject player;
    public PlayerControls playerScript;

    [Header("------UI stuff------")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject winMenu;
    public GameObject looseMenu;

    public int jumpCost;
    public int coins;
    public bool isPaused;
    float timeScaleOrig;
    int enemyAmount = 0;
    public GameObject playerSpawnPos;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
        timeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn");

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            if (isPaused)
                pause();
            else
                unPause();
        }
    }

    public void addCoins(int amount)
    {
        coins += amount;
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }


    public void updateEnemyCount(int amount)
    {
        enemyAmount += amount;
        if (enemyAmount <= 0)
        {
            winMenu.SetActive(true);
            pause();
            activeMenu = winMenu;
        }

    }
}
