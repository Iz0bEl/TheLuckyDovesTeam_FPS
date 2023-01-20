using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool entered;

    private void Start()
    {

        entered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!entered && other.CompareTag("Player"))
        {
            GameManager.instance.playerSpawnPos = gameObject;
            StartCoroutine(popup());
            entered = true;
        }
    }

    IEnumerator popup()
    {
        GameManager.instance.checkpointReached.SetActive(true);
        yield return new WaitForSeconds(5);
        GameManager.instance.checkpointReached.SetActive(false);
    }
}
