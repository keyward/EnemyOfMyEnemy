using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {



    [Header("Seconds before enemies appear")]
    public float initialDelay; // indicates how many seconds after activation enemies will spawn //

    [Header("how many enemies appear")]
    public int enemiesToSpawn; // how many enemies will spawn from this one point //

    [Header("enemy type to spawn")]
    public GameObject enemyPrefab; // what type of enemy will appear from this point //

    [Header("time until next spawn")]
    public float spawnTimeFrequency = 2f; // how long in between spawns will appear //

   

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
