using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] int pushBackAmount;
    [SerializeField] Renderer model;

    private void Start()
    {
        model.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.pushBackInput(transform.forward * pushBackAmount);
        }

    }
}
