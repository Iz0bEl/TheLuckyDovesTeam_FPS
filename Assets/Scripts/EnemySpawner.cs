using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] int timer;
    [SerializeField] Transform spawnPosition;

    bool isSpawning;
    bool playerInRange;
    int enemiesSpawned;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateEnemyCount(enemiesToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && enemiesSpawned < enemiesToSpawn)
        {
            StartCoroutine(Spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    IEnumerator Spawn()
    {
        isSpawning = true;

        Instantiate(enemy, spawnPosition.position, enemy.transform.rotation);

        yield return new WaitForSeconds(timer);

        isSpawning = false;
    }
}
