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
    [SerializeField] int jumpHeight;
    [SerializeField] int gravityValue;
    [SerializeField] int jumpsMax;
    [SerializeField] int currentWeapon;

    [Header("----- single Fire gun Stats -----")]
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

    int HPOG;
    bool isShooting;

    int jumpedTimes;
    private Vector3 playerVelocity;
    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        rifleEquiped = true;
        SetPlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            movement();

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
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);



        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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
                        hitInfo.collider.GetComponent<IDamage>().takeDamage(shotGunDamagePerBullet);
                    }
                    Debug.DrawRay(GameManager.instance.player.transform.position, hitInfo.point);
                }
                Debug.Log("Shotgun shoot");

            }

            yield return new WaitForSeconds(ShotGunshootRate);

            isShooting = false;

        }

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


    void swapWeapons()
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

    public void setPlayerSpawnPoint()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void resetPlayerHP()
    {
        HP = HPOG;
    }

    public void SetPlayerPos()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

}
