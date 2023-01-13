using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class Explosion : MonoBehaviour
{

    
    [SerializeField] AudioClip ExplosionAudio;
        
    //[SerializeField] float speed;
    //[SerializeField] int timer;
    [SerializeField] int damage;
    [SerializeField] GameObject targets;
    [SerializeField] int pushBackTime;
    [SerializeField] int pushBackAmount;

    public Vector3 pushBack;

   


    void Start()
    {

        Destroy(gameObject, 2f);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targets = other.gameObject;

            targets.GetComponentInParent<EnemyAI>().takeDamage(damage);
            
            GameManager.instance.playerScript.pushBackInput((other.transform.position - transform.position) * pushBackAmount);
            

        }
    }

        
    

    // possibly add a smoke particle effect to missile
    //explode with audio
    //push enemies/player away from impact
}
