using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    public GameObject weaponSelectionScreen;

    [SerializeField] TextMeshProUGUI currentAmmo;
    [SerializeField] TextMeshProUGUI MaxAmmo;


    [Header("-------------------")]
    public Image playerHPbar;
    public Image playerAbilityCooldown;
    [SerializeField] TextMeshProUGUI enemiesLeft;
    public Transform iconPos;
    public Transform iconPos1;
    public Transform iconPos2;
    public Transform iconPos3;
    public Transform iconPos4;
    public GameObject checkpointReached;

    public bool sniperScopeActive = false;

    [Header("------Collectables------")]
    public int jumpCost;
    public int speedCost;
    public int enemyCount;
    public float coins;


    public bool isPaused;
    public float timeScaleOrig;
    public GameObject playerSpawnPos;
    //[SerializeField] int enemyAmount = 0;
    //public GameObject playerSpawnPos;
    // Start is called before the first frame update
    void Awake()
    {
        
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
        timeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn");
        

    }
    private void Start()
    {
        Application.targetFrameRate = 144;  
       
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

    

    public void updateAmmo()
    {
        currentAmmo.text = playerScript.bulletsInClip.ToString();
        MaxAmmo.text = playerScript.maxAmmo.ToString();
    }

    public void addCoins(float amount)
    {
        coins += amount;
    }

    public void pause()
    {
        //timeScaleOrig = Time.timeScale;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //AudioSource[] audios = FindObjectsOfType<AudioSource>();
        //foreach (AudioSource sounds in audios)
        //{
         //   sounds.Pause();
       // }

    }
    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu?.SetActive(false);
        activeMenu = null;

       // AudioSource[] audios = FindObjectsOfType<AudioSource>();
       // foreach (AudioSource sounds in audios)
       // {
           // sounds.Play();
       // }
    }


    public void updateEnemyCount(int amount)
    {
        //making the win condition killing the final boss

        //enemyCount += amount;

        //enemiesLeft.text = enemyCount.ToString("F0");

        //if (enemyCount <= 0)
        //{
        //    winMenu.SetActive(true);
        //    pause();
        //    activeMenu = winMenu;
        //}

    }

    public void DisplayWinScreen()
    {
        winMenu.SetActive(true);
        pause();
        activeMenu = winMenu;
    }

    void ShowSniperScope()
    {

        if (playerScript.gunList.Count > 0)
        {

            if (Input.GetMouseButtonDown(1) && playerScript.gunList[playerScript.selectedGun].isSniper)
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
}
