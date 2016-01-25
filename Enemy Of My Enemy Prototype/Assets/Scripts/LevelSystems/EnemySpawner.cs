using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {


    public int enemiesToSpawn;
    public GameObject enemyPrefab;
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
