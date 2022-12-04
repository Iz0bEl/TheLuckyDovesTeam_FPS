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

    [Header("----- single Fire gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    [Header("----- Shotgun Stats -----")]
    [SerializeField][Range(0.1f,0.5f)] float shotGunDamagePerBullet;
    [SerializeField] float ShotGunshootRate;
    [SerializeField] int ShotGunRange;


    [Header("----- Sniper Rifle Stats -----")]
    [SerializeField] [Range(1.0f, 5.0f)] int SniperDamage;
    [SerializeField] float SnipershootRate;
    [SerializeField] int SniperRange;


    [SerializeField] bool shotgunEquiped;
    [SerializeField] bool rifleEquiped;
    [SerializeField] bool sniperEquiped;

    bool isShooting;

    int jumpedTimes;
    private Vector3 playerVelocity;
    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement();
       
        StartCoroutine(shoot());
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
        if (!isShooting && Input.GetButton("Shoot"))
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

        //trying to emulate a shotgun by offsetting the raycast
        //Vector3 shotgunDirection = transform.forward;
        //shotgunDirection.x += Random.Range(-1, 1) * shotgunBulletSpread;
        //shotgunDirection.y += Random.Range(-1, 1) * shotgunBulletSpread;
        //shotgunDirection.z += Random.Range(-1, 1) * shotgunBulletSpread;

        //rb.velocity = shotgunDirection


    }
}
