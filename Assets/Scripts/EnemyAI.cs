using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("--- Enemy Components ---")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    

    [Header("--- Enemy Stats ---")]
    public float HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPOS;

    [Header("--- Enemy Weapon Stats ---")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    float HPOG;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        HPOG = HP;        
        GameManager.instance.updateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            CanSeePlayer();
        }

        if((agent.remainingDistance - agent.stoppingDistance) <= .1)
        {            
            EnemyAnimationController.StartIdleAnimation(gameObject);
        }
        
    }

    void CanSeePlayer()
    {
        playerDirection = (GameManager.instance.player.transform.position - headPOS.position);
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPOS.position, playerDirection);

        RaycastHit hit;

        if (Physics.Raycast(headPOS.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
           {
               agent.SetDestination(GameManager.instance.player.transform.position);

                if (!isShooting && angleToPlayer <= 15)
                    StartCoroutine(Shoot());

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
            }
        }
    }

    void FacePlayer()
    {
        playerDirection.y = 0;

        Quaternion rot = Quaternion.LookRotation(playerDirection);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.destination = gameObject.transform.position;
            EnemyAnimationController.StartIdleAnimation(gameObject);
            
        }
    }

    public void takeDamage(float dmg)
    {
        HP -= dmg;

        agent.SetDestination(GameManager.instance.player.transform.position);

        if(!playerInRange)
        {
            EnemyAnimationController.StartRunAnimation(gameObject);
        }
        StartCoroutine(FlashDamage());

        if (HP <= 0)
        {
            GameManager.instance.addCoins(HPOG);
            GameManager.instance.updateEnemyCount(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator FlashDamage()
    {
        Debug.Log("FlashDamage");
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;

    }

    IEnumerator Shoot()
    {
        isShooting = true;
        
        if(agent.stoppingDistance >= Vector3.Distance(transform.position, GameManager.instance.player.transform.position))
        {
            EnemyAnimationController.StartShootingAnimation(gameObject);
        }
        else
        {
            EnemyAnimationController.StartRunAnimation(gameObject);
        }

        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    
}
