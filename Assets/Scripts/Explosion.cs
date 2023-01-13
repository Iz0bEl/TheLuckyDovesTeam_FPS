using UnityEngine;

public class Explosion : MonoBehaviour
{


    [SerializeField] AudioClip ExplosionAudio;

    //[SerializeField] float speed;
    //[SerializeField] int timer;
    [SerializeField] int damage;
    [SerializeField] GameObject targets;

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

        }
    }




    // possibly add a smoke particle effect to missile
    //explode with audio
    //push enemies/player away from impact
}
