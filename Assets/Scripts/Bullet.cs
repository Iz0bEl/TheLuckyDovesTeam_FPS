using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (GameManager.instance.player.transform.position - transform.position) * speed;       
        Destroy(gameObject, timer);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            GameManager.instance.playerScript.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
