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
    [SerializeField] float pushBackTime;

    [Header("-----Audio-----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)] [SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerJump;
    [Range(0, 1)] [SerializeField] float playerJumpVol;
    [SerializeField] AudioClip[] playerStep;
    [Range(0, 1)] [SerializeField] float playerSetpVol;

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

    [Header("----- Equipped Weapon Stats -----")]
    public List<GunStats> gunList = new List<GunStats>();
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject hitEffect;


    int HPORG;
    bool isShooting;
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
        isSprinting = false;
        abilityTimeSlow = true;
        onCooldown = false;
        SetPlayerPos();
        UpdatePlayerHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            movement();

            if (!stepIsPlaying && move.magnitude > 0.3f && controller.isGrounded)
            {
                StartCoroutine(playSteps());
            }

            StartCoroutine(ability());

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
            aud.PlayOneShot(playerJump[Random.Range(0, playerJump.Length)], playerJumpVol);
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
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;
            RaycastHit hit;

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


    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootRate = gunList[selectedGun].shootRate;
        shootDistance = gunList[selectedGun].shootDistance;


        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
