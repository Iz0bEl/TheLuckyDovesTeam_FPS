using System.Collections;
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
    public GameObject SniperScopeUI;
    public GameObject playerFlashDamage;
    public GameObject timeSlowScreen;

    public bool sniperScopeActive = false;
   
    public int jumpCost;
    public float coins;
    public bool isPaused;
    public float timeScaleOrig;
    public GameObject playerSpawnPos;
    [SerializeField] int enemyAmount = 0;
    //public GameObject playerSpawnPos;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
        timeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn");
        //player.transform.position = playerSpawnPos.transform.position;

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
        if (Input.GetMouseButton(1) && !isPaused)
        {
            ShowSniperScope();
        }


    }

    public void addCoins(float amount)
    {
        coins += amount;
    }

    public void pause()
    {
        timeScaleOrig = Time.timeScale;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu?.SetActive(false);
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

    void ShowSniperScope()
    {
        if (Input.GetMouseButtonDown(1) && playerScript.sniperEquiped)
        {
            sniperScopeActive = !sniperScopeActive;

            if (sniperScopeActive)
            {
                SniperScopeUI.SetActive(true);
                Camera.main.fieldOfView = 10;
               
            }
            else
            {
                SniperScopeUI.SetActive(false);
                Camera.main.fieldOfView = 60;
                
            }

            
        }
    }
}
