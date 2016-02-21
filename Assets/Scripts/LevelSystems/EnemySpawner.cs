using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {


    [Header("seconds to wait before spawning enemies")]
    public float delayBeforeSpawn;

    [Header("amount of enemies in spawn trigger")]
    public int enemiesToSpawn;

    [Header("enemy type to spawn")]
    public GameObject enemyPrefab;

    [Header("seconds between each enemy spawning")]
    public float spawnTimeFrequency = 2f;


	void Start ()
    {
        StartCoroutine(SpawnEnemy());
	}
	
    IEnumerator SpawnEnemy()
    {
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnTimeFrequency);
        }

        Destroy(gameObject);
    }
}
