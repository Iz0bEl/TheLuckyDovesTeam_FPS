using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("--- Enemy Components ---")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] int pushBackTime;


    [Header("--- Enemy Stats ---")]
    public float HP;
    public int EnemyDamage;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int animTransSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPOS;

    [Header("--- Enemy Weapon Stats ---")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] int roamingDistance;
    [SerializeField] Transform headShotPos;


    [Header("--- Enemy UI ---")]
    [SerializeField] Image HPBar;
    [SerializeField] GameObject UI;

    [Header("---- Hand Colliders ----")]
    [SerializeField] BoxCollider LeftHand;
    [SerializeField] BoxCollider RightHand;


    [Header("--- Item drop ---")]
    [SerializeField] GameObject AmmoDrop;

    float HPOG;
    bool isAttacking;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    Vector3 startingPosition;
    float stoppingDistanceOG;
    public Vector3 pushBack;




    // Start is called before the first frame update
    void Start()
    {
        HPOG = HP;

        stoppingDistanceOG = agent.stoppingDistance;

        startingPosition = transform.position;

        updateHPBar();

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed));

        if (playerInRange)
        {
            CanSeePlayer();
        }
        //else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position)
        //{
        //    EnemyRoaming();
        //}


    }

    void CanSeePlayer()
    {
        playerDirection = (GameManager.instance.player.transform.position - headPOS.position);
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        // Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPOS.position, playerDirection);

        RaycastHit hit;

        if (Physics.Raycast(headPOS.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistanceOG;

                agent.SetDestination(GameManager.instance.player.transform.position);

                FacePlayer();

                if (agent.hasPath)
                {
                    if (!isAttacking && angleToPlayer <= 90)
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            if (!isAttacking)
                                StartCoroutine(AttackPlayer());
                        }

                    }

                }

            }
        }
    }

    void EnemyRoaming()
    {
        agent.stoppingDistance = 0;

        Vector3 randomDirection = Random.insideUnitSphere * roamingDistance;
        randomDirection += startingPosition;

        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(randomDirection.x, 0, randomDirection.z), out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        if (hit.position != null)
        {
            agent.CalculatePath(hit.position, path);

        }

        agent.SetPath(path);
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

        }
    }

    public void takeDamage(int dmg)
    {
        //made it so the RPG cannot crit
        if (headShotPos && !GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun].isRPG)
        {
            HP -= dmg * 2;

            updateHPBar();

            UI.SetActive(true);

            agent.SetDestination(GameManager.instance.player.transform.position);


            StartCoroutine(FlashDamage());

            if (HP <= 0)
            {
                GameManager.instance.addCoins(HPOG);
                GameManager.instance.updateEnemyCount(-1);
                Destroy(gameObject);
            }
        }
        else
        {
            HP -= dmg;

            //updateHPBar();

            // UI.SetActive(true);
            FacePlayer();

            agent.SetDestination(GameManager.instance.player.transform.position);


            StartCoroutine(FlashDamage());

            if (HP <= 0)
            {
                GameManager.instance.addCoins(HPOG);
                GameManager.instance.updateEnemyCount(-1);
                if(gameObject.tag == "BossGhoul")
                {
                    GameManager.instance.DisplayWinScreen();
                }
                Destroy(gameObject);
            }
        }
    }

    IEnumerator FlashDamage()
    {
        Debug.Log("FlashDamage");
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;

    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        LeftHand.enabled= true;
        RightHand.enabled = true;
        anim.SetBool("isAttacking", isAttacking);

        yield return new WaitForSeconds(1.5f);


        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        LeftHand.enabled = false;
        RightHand.enabled = false;

    }

    void updateHPBar()
    {
       // HPBar.fillAmount = (float)HP / (float)HPOG;
    }
    void OnDestroy()
    {
        int num = Random.Range(0, 100);

        if (num < 5)
        {
            Instantiate(AmmoDrop, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);

        }
    }
}
