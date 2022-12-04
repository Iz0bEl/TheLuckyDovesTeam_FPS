using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;

    [Header("------ Shotgun Spread ------")]
    [Range(1, 5)] [SerializeField] int shotgunBulletSpread;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;       
        Destroy(gameObject, timer);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //+++++waiting for gameManager to be setup++++++
            //gameManager.instance.playerScript.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
