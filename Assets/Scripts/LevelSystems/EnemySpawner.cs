using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {



    [Header("seconds before enemies appear")]
    public float spawnDelay;

    [Header("amount of enemies that spawn")]
    public int enemiesToSpawn;

    [Header("seconds between enemy spawn")]
    public float spawnTimeFrequency = 2f;

    [Header("type of enemy to spawn")]
    public GameObject enemyPrefab;


    void Start ()
    {
        StartCoroutine(SpawnEnemy());
	}
	
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnDelay);

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnTimeFrequency);
        }

        Destroy(gameObject);
    }
}
