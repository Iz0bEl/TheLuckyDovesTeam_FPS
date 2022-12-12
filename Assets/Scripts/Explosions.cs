using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour
{
    [SerializeField] int pushBackAmount;
    [SerializeField] bool push;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (push)
                GameManager.instance.playerScript.pushBackInput((other.transform.position - transform.position) * pushBackAmount);
            else
                GameManager.instance.playerScript.pushBackInput((transform.position - other.transform.position) * pushBackAmount);
        }

    }
}
