using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] int timeAlive;
    [SerializeField]  GameObject explosionEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Camera.main.transform.forward * speed;
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Vector3.up);

        Destroy(this.gameObject, timeAlive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
