using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public static void StartRunAnimation(GameObject gameObject)
    {
        gameObject.GetComponent<Animator>().Play("Running");
    }
    
    public static void StartIdleAnimation(GameObject gameObject)
    {
        gameObject.GetComponent<Animator>().Play("Combat Idle");
    }

    public static void StartShootingAnimation(GameObject gameObject)
    {
        gameObject.GetComponent<Animator>().Play("Shooting");
    }
}
