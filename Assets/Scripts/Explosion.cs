using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    
    [SerializeField] AudioClip ExplosionAudio;
        
    //[SerializeField] float speed;
    //[SerializeField] int timer;
    [SerializeField] int damage;
    [SerializeField] List<GameObject> targets = new List<GameObject>();

   
    
    void Start()
    {

        Destroy(gameObject, 2f);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if(!targets.Contains(other.gameObject))
                targets.Add(other.gameObject);
            
        }
        
    }

    // possibly add a smoke particle effect to missile
    //explode with audio
    //push enemies/player away from impact
}
