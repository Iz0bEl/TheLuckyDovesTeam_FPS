using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
        
    [SerializeField] int speed;
    [SerializeField] int timer;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (GameManager.instance.player.transform.position - transform.position) * speed;
        Destroy(gameObject, timer);
        damage = GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun].shootDamage;
    }

    public void OnTriggerEnter(Collider other)
    {
        
        GameManager.instance.playerScript.takeDamage(damage);
        
        Destroy(gameObject);
    }

    // possibly add a smoke particle effect to missile
    //hit a collider
    //explode with audio
    //push enemies/player away from impact
}
