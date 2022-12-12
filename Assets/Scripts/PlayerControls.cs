using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerSpeed;
    [SerializeField] int playerSprintSpeed;
    [SerializeField] int jumpHeight;
    [SerializeField] int gravityValue;
    [SerializeField] int jumpsMax;
    [SerializeField] int currentWeapon;
    bool isSprinting;

    [Header("----- Wall Running -----")]
    [Range(1, 10)] [SerializeField] int wallRunBoost; // not implemented will give players a boost to y velocity on contact with wall
    [Range(1, 10)] [SerializeField] int gravityScale;
    [Range(5, 15)] [SerializeField] int wallJumpSpeed; // not working yet

    [Header("----- Abilities -----")]
    public bool abilityTimeSlow;
    public bool timeSlowed;
    [SerializeField] float timeSlowScale;
    [Range(1, 5)] [SerializeField] int timeSlowTimer;
    [Range(5, 10)] [SerializeField] int timeSlowCooldown;
    public bool onCooldown;

    [Header("----- Auto Rifle Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    public bool rifleEquiped;

    [Header("----- Shotgun Stats -----")]
    [SerializeField][Range(0.1f,0.5f)] float shotGunDamagePerBullet;
    [SerializeField] float ShotGunshootRate;
    [SerializeField] int ShotGunRange;
    public bool shotgunEquiped;


    [Header("----- Sniper Rifle Stats -----")]
    [SerializeField] [Range(1.0f, 5.0f)] int SniperDamage;
    [SerializeField] float SnipershootRate;
    [SerializeField] int SniperRange;
    public bool sniperEquiped;

    int HPORG;
    bool isShooting;

    int jumpedTimes;
    private Vector3 playerVelocity;
    private bool wallRight, wallLeft;
    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        HPORG = HP;
        isSprinting = false;
        abilityTimeSlow = true;
        onCooldown = false;
        rifleEquiped = true;
        SetPlayerPos();
        UpdatePlayerHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            movement();
            StartCoroutine(ability());
            StartCoroutine(shoot());

            if (Input.GetKey("1"))
            {
                currentWeapon = 1;
                swapWeapons();
            }
            else if (Input.GetKey("2"))
            {
                currentWeapon = 2;
                swapWeapons();
            }
            else if (Input.GetKey("3"))
            {
                currentWeapon = 3;
                swapWeapons();

            }
        }
       

    }

    void movement()
    {
        checkForWall();
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        sprint();
        if (isSprinting)
        {
            controller.Move(move * Time.deltaTime * playerSprintSpeed);
        }
        else
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
        }

        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        // gravity has moved to wallphysics -kayla
        wallPhysics();
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (!isSprinting && Input.GetButtonDown("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            isSprinting = true;
        }
        else if (isSprinting && Input.GetButtonDown("Sprint") || (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
        {
            isSprinting = false;
        }
    }

    void checkForWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 1f))
        {
            if (hit.collider.gameObject.tag == "Wall" && Input.GetKey(KeyCode.D))
            {
                wallRight = true;
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 1f))
        {
            if (hit.collider.gameObject.tag == "Wall" && Input.GetKey(KeyCode.A))
            {
                wallLeft = true;
            }
        }
        else
        {
            wallRight = false;
            wallLeft = false;
        }
    }

    void wallPhysics()
    {
        if (wallRight)
        {
                      
                playerVelocity.y -= (gravityValue / gravityScale) * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                playerVelocity = transform.right * wallJumpSpeed * Time.deltaTime;
                playerVelocity.y += jumpHeight;
            }
        }
        else if (wallLeft)
        {
                    
                playerVelocity.y -= (gravityValue / gravityScale) * Time.deltaTime;

            
            if (Input.GetButtonDown("Jump"))
            {
                playerVelocity = -transform.right * wallJumpSpeed * Time.deltaTime;
                playerVelocity.y += jumpHeight;
            }
        }
        else
        {
            playerVelocity.y -= gravityValue * Time.deltaTime;
        }
    }

    IEnumerator ability()
    {
        if (abilityTimeSlow)
        {
            if (!timeSlowed && !onCooldown && Input.GetButtonDown("Ability") )
            {
                timeSlowed = true;
                GameManager.instance.timeScaleOrig = Time.timeScale;
                Time.timeScale = timeSlowScale;
                GameManager.instance.timeSlowScreen.SetActive(true);
                yield return new WaitForSecondsRealtime(timeSlowTimer);
                GameManager.instance.timeSlowScreen.SetActive(false);
                Time.timeScale = GameManager.instance.timeScaleOrig;
                timeSlowed = false;
                onCooldown = true;
                yield return new WaitForSeconds(timeSlowCooldown);
                onCooldown = false;
            }
            else if (timeSlowed && Input.GetButtonDown("Ability"))
            {
                GameManager.instance.timeSlowScreen.SetActive(false);
                Time.timeScale = GameManager.instance.timeScaleOrig;
                timeSlowed = false;
                onCooldown = true;
                yield return new WaitForSeconds(timeSlowCooldown);
                onCooldown = false;
            }
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        UpdatePlayerHPBar();
        StartCoroutine(playerDamageFlash());

        if (HP <= 0)
        {
            GameManager.instance.pause();
            GameManager.instance.looseMenu.SetActive(true);
            GameManager.instance.activeMenu = GameManager.instance.looseMenu;
        }
    }

    IEnumerator playerDamageFlash()
    {
        GameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerFlashDamage.SetActive(false);
    }

    IEnumerator shoot()
    {
        //Rifle mechanic
        if (!isShooting && Input.GetButton("Shoot") && rifleEquiped)
        {
            isShooting = true;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            Debug.Log("I shoot");
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }

        
        if (!isShooting && Input.GetButton("Shoot") && shotgunEquiped)
        {
            isShooting = true;
            RaycastHit hitInfo;

            //need to shoot 5 raycast within a certain spread
            for (int i = 0; i < 5; i++)
            {
                //Thinking that if I subtract a random number between 0.01 and 0.02 from the .5 which is middle of screen, it will ofset the bullets accordingly
                
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f - Random.Range(0.01f, 0.02f), 0.5f - Random.Range(0.01f, 0.02f))), out hitInfo, ShotGunRange))
                {
                    if (hitInfo.collider.GetComponent<IDamage>() != null)
                    {
                        //this if statement fixed a bug that caused the take damage to get called even when the cube was dead
                        //causing the game manager's count of enemies to be inaccurate
                        if (hitInfo.collider.GetComponent<EnemyAI>().HP >= 0)
                        {
                            hitInfo.collider.GetComponent<IDamage>().takeDamage(shotGunDamagePerBullet);
                        }
                        
                    }
                    Debug.DrawRay(GameManager.instance.player.transform.position, hitInfo.point);
                }
                Debug.Log("Shotgun shoot");

            }

            yield return new WaitForSeconds(ShotGunshootRate);

            isShooting = false;

        }

        //Sniper shooting
        if (!isShooting && Input.GetButton("Shoot") && sniperEquiped)
        {

            isShooting = true;
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, SniperRange))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(SniperDamage);
                }
            }

            Debug.Log("Sniper shoot");
            yield return new WaitForSeconds(SnipershootRate);
            isShooting = false;

        }

    }


    public void swapWeapons()
    {
        switch (currentWeapon)
        {
            case 1:
            {
                    rifleEquiped = true;
                    shotgunEquiped = false;
                    sniperEquiped = false;
                    GameManager.instance.SniperScopeUI.SetActive(false);
                    Camera.main.fieldOfView = 60;
                    break;
            }
            case 2:
            {
                    rifleEquiped = false;
                    shotgunEquiped = true;
                    sniperEquiped = false;
                    GameManager.instance.SniperScopeUI.SetActive(false);
                    Camera.main.fieldOfView = 60;
                    break;
            }
            case 3:
            {
                    rifleEquiped = false;
                    shotgunEquiped = false;
                    sniperEquiped = true;
                    break;
            }
            default:
                break;
        }
    }

    public void addJump(int amount)
    {
        jumpsMax += amount;
        GameManager.instance.coins -= GameManager.instance.jumpCost;
    }

    public void addSpeed(int amount)
    {
        playerSpeed += amount;
        GameManager.instance.coins -= GameManager.instance.speedCost;
    }

    public void setPlayerSpawnPoint()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void resetPlayerHP()
    {
        HP = HPORG;
    }

    public void SetPlayerPos()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void UpdatePlayerHPBar()
    {
        GameManager.instance.playerHPbar.fillAmount = (float)HP / (float)HPORG;
    }

}
