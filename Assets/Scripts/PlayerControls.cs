using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool toggleSprint;
    bool isSprinting;
    [SerializeField] float pushBackTime;

    [Header("-----Audio-----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip gunShot;
    [Range(0, 1)] [SerializeField] float gunShotVol;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)] [SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerJump;
    [Range(0, 1)] [SerializeField] float playerJumpVol;
    [SerializeField] AudioClip[] playerStep;
    [Range(0, 1)] [SerializeField] float playerSetpVol;

    [Header("----- Wall Running -----")]
    [Range(1, 10)] [SerializeField] int gravityScale;
    [Range(5, 15)] [SerializeField] int wallJumpSpeed;
    [SerializeField] int wallJumpPushTime;
    GameObject firstWall;
    GameObject secondWall;
    Vector3 wallNormal;
    bool toggleWall;
    bool isAttached;
    bool newWall;
    public Vector3 wallJumpPush;

    [Header("----- Abilities -----")]
    public bool abilityTimeSlow;
    public bool timeSlowed;
    [SerializeField] float timeSlowScale;
    [Range(1, 5)] [SerializeField] int timeSlowTimer;
    [Range(5, 10)] [SerializeField] int abilityCooldown;
    public bool onCooldown;
    float cooldownTimer;

    [Header("----- Equipped Weapon Stats -----")]
    public List<GunStats> gunList = new List<GunStats>();
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject hitEffect;
    GameObject gunSelectedUI;

    [SerializeField] bool[] gunOrder;
    [SerializeField] Transform GunModelPosition;


    int HPORG;
    bool isShooting;
    bool IsRocketFiring;
    public int selectedGun;

    int jumpedTimes;
    private Vector3 playerVelocity;
    private bool wallRight, wallLeft;
    Vector3 move;
    public Vector3 pushBack;
    bool stepIsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        HPORG = HP;
        toggleSprint = true;
        isSprinting = false;
        abilityTimeSlow = true;
        onCooldown = false;
        cooldownTimer = abilityCooldown;
        SetPlayerPos();
        UpdatePlayerHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            wallJumpPush = Vector3.Lerp(wallJumpPush, Vector3.zero, Time.deltaTime * wallJumpPushTime);
            movement();

            if (!stepIsPlaying && move.magnitude > 0.3f && controller.isGrounded)
            {
                StartCoroutine(playSteps());
            }

            StartCoroutine(ability());
            if (onCooldown)
            {
                abilityCooldownTimer();
            }

            if (gunList.Count > 0)
            {
                StartCoroutine(shoot());
                gunSelect();
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
            toggleWall = false;
            firstWall = null;
            secondWall = null;
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

        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax && !wallLeft && !wallRight)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(playerJump[Random.Range(0, playerJump.Length)], playerJumpVol);
        }

        // gravity has moved to wallphysics -kayla
        wallPhysics();
        controller.Move((playerVelocity + wallJumpPush) * Time.deltaTime);
    }

    void sprint()
    {
        if (toggleSprint)
        {
            if (!isSprinting && Input.GetButtonDown("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                isSprinting = true;
            }
            else if (isSprinting && (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                isSprinting = false;
            }
        }
        else if (!toggleSprint)
        {
            if (!isSprinting && Input.GetButton("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                isSprinting = true;
            }
            else if (isSprinting && ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || !Input.GetButton("Sprint")))
            {
                isSprinting = false;
            }
        }
    }

    void checkForWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 1f) && hit.collider.gameObject.tag == "Wall" && Input.GetKey(KeyCode.D))
        {

            wallRight = true;
            if (!toggleWall && isAttached == false)
            {
                firstWall = hit.collider.gameObject;
                if (firstWall == secondWall)
                {
                    newWall = false;
                }
                else
                {
                    newWall = true;
                }
                isAttached = true;
                toggleWall = true;
            }
            else if (toggleWall && isAttached == false)
            {
                secondWall = hit.collider.gameObject;
                if (firstWall == secondWall)
                {
                    newWall = false;
                }
                else
                {
                    newWall = true;
                }
                isAttached = true;
                toggleWall = false;
            }
            wallNormal = hit.normal;
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 1f) && hit.collider.gameObject.tag == "Wall" && Input.GetKey(KeyCode.A))
        {

            wallLeft = true;
            if (!toggleWall && isAttached == false)
            {
                firstWall = hit.collider.gameObject;
                if (firstWall == secondWall)
                {
                    newWall = false;
                }
                else
                {
                    newWall = true;
                }
                isAttached = true;
                toggleWall = true;
            }
            else if (toggleWall && isAttached == false)
            {
                secondWall = hit.collider.gameObject;
                if (firstWall == secondWall)
                {
                    newWall = false;
                }
                else
                {
                    newWall = true;
                }
                isAttached = true;
                toggleWall = false;
            }
            wallNormal = hit.normal;
        }
        else
        {
            wallRight = false;
            wallLeft = false;
            isAttached = false;
        }
    }

    void wallPhysics()
    {
        if (isAttached)
        {
            if (newWall && playerVelocity.y < 0)
            {
                playerVelocity.y = 0;

            }
            newWall = false;
            playerVelocity.y -= (gravityValue / gravityScale) * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                if (isSprinting)
                {
                    wallJumpPush = wallNormal * wallJumpSpeed * 2;
                }
                else
                {
                    wallJumpPush = wallNormal * wallJumpSpeed;
                }
                playerVelocity.y = 0;
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
            if (!timeSlowed && !onCooldown && Input.GetButtonDown("Ability"))
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
            }
            else if (timeSlowed && Input.GetButtonDown("Ability"))
            {
                GameManager.instance.timeSlowScreen.SetActive(false);
                Time.timeScale = GameManager.instance.timeScaleOrig;
                timeSlowed = false;
                onCooldown = true;
            }
        }
    }

    public void abilityCooldownTimer()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0f)
        {
            onCooldown = false;
            GameManager.instance.playerAbilityCooldown.fillAmount = 0f;
        }
        else
        {
            GameManager.instance.playerAbilityCooldown.fillAmount = cooldownTimer / abilityCooldown;
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        aud.PlayOneShot(playerHurt[Random.Range(0, playerHurt.Length)], playerHurtVol);
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
        if (!isShooting && Input.GetButton("Shoot") && !gunList[selectedGun].isShotgun)
        {
            isShooting = true;
            RaycastHit hit;
            aud.PlayOneShot(gunList[selectedGun].gunShot, gunShotVol);


            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    if (hit.collider.GetComponent<EnemyAI>().HP > 0)
                        hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            Debug.Log("I shoot");
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }


        if (!isShooting && Input.GetButton("Shoot") && gunList[selectedGun].isShotgun)
        {
            isShooting = true;
            RaycastHit hitInfo;
            aud.PlayOneShot(gunList[selectedGun].gunShot, gunShotVol);
            //need to shoot 5 raycast within a certain spread
            for (int i = 0; i < 5; i++)
            {
                //Thinking that if I subtract a random number between 0.01 and 0.02 from the .5 which is middle of screen, it will ofset the bullets accordingly

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f - Random.Range(0.01f, 0.02f), 0.5f - Random.Range(0.01f, 0.02f))), out hitInfo, shootDistance))
                {
                    if (hitInfo.collider.GetComponent<IDamage>() != null)
                    {
                        //this if statement fixed a bug that caused the take damage to get called even when the cube was dead
                        //causing the game manager's count of enemies to be inaccurate
                        if (hitInfo.collider.GetComponent<EnemyAI>().HP >= 0)
                        {
                            hitInfo.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                        }

                    }
                    Debug.DrawRay(GameManager.instance.player.transform.position, hitInfo.point);
                }
                Debug.Log("Shotgun shoot");

            }

            yield return new WaitForSeconds(shootRate);

            isShooting = false;

        }

    }

    IEnumerator ShootRocketLauncher()
    {

        IsRocketFiring = true;

        if (!IsRocketFiring && Input.GetButton("Shoot") && !gunList[selectedGun].isShotgun)
        {
            IsRocketFiring = true;
            RaycastHit hit;
            aud.PlayOneShot(gunList[selectedGun].gunShot, gunShotVol);


            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    if (hit.collider.GetComponent<EnemyAI>().HP > 0)
                        hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            Debug.Log("RocketLaunched!");
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }


        yield return new WaitForSeconds(shootRate);

        IsRocketFiring = false;
    }

    IEnumerator playSteps()
    {
        stepIsPlaying = true;

        aud.PlayOneShot(playerStep[Random.Range(0, playerStep.Length)], playerSetpVol);

        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        stepIsPlaying = false;


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
        UpdatePlayerHPBar();
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

    public void pushBackInput(Vector3 dir)
    {
        pushBack = dir;
    }

    public void gunPickup(GunStats gunStat)
    {

        shootDamage = gunStat.shootDamage;
        shootRate = gunStat.shootRate;
        shootDistance = gunStat.shootDistance;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        gunList.Add(gunStat);

        selectedGun = gunList.Count - 1;
        

        if (gunList.Count != 0)
        {
           for (int i = 0; i < gunList.Count; i++)
            {
                if (!gunOrder[i])
                {
                    gunOrder[i] = true;
                    if (i == 0)
                    {
                        Instantiate(gunList[selectedGun].UI, GameManager.instance.iconPos);
                        gunList[selectedGun].slotNumber = gunList.Count;

                        break;
                    }
                    else if (i == 1)
                    {
                        Instantiate(gunList[selectedGun].UI, GameManager.instance.iconPos1);
                        gunList[selectedGun].slotNumber = gunList.Count;
                        break;
                    }
                    else if (i == 2)
                    {
                        Instantiate(gunList[selectedGun].UI, GameManager.instance.iconPos2);
                        gunList[selectedGun].slotNumber = gunList.Count;
                        break;
                    }
                    else if( i == 3)
                    {
                        Instantiate(gunList[selectedGun].UI, GameManager.instance.iconPos3);
                        gunList[selectedGun].slotNumber = gunList.Count;
                        break;
                    }
                }
            }
            if (gunSelectedUI == null)
            {
                gunSelectedUI = GameObject.FindGameObjectWithTag(gunList[selectedGun].tag);
            }
            else
            {
                gunSelectedUI.GetComponent<Image>().color = Color.white;
                gunSelectedUI = GameObject.FindGameObjectWithTag(gunList[selectedGun].tag);
                gunSelectedUI.GetComponent<Image>().color = Color.cyan;
            }
            changeGun();
        }
    }

    void gunSelect()
    {
        if (gunList.Count != 0 && Input.GetButtonDown("Gun1"))
        {
            selectedGun = 0;
            changeGun();

        }
        else if (gunList.Count > 1 && Input.GetButtonDown("Gun2"))
        {
            selectedGun = 1;
            changeGun();

        }
        else if (gunList.Count > 2 && Input.GetButtonDown("Gun3"))
        {
            selectedGun = 2;
            changeGun();

        }
        else if(gunList.Count > 3 && Input.GetButtonDown("Gun4"))
        {
            selectedGun = 3;
            changeGun();
        }


    }

    void changeGun()
    {
        
        gunSelectedUI.GetComponent<Image>().color = Color.white;
        gunSelectedUI = GameObject.FindGameObjectWithTag(gunList[selectedGun].tag);
        gunSelectedUI.GetComponent<Image>().color = Color.cyan;

        shootDamage = gunList[selectedGun].shootDamage;
        shootRate = gunList[selectedGun].shootRate;
        shootDistance = gunList[selectedGun].shootDistance;


        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        if(gunSelectedUI.tag == "RocketLauncherUI")
        {
            GunModelPosition.localRotation = new Quaternion(0,180,0,0);
        }
        else
        {
            GunModelPosition.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
