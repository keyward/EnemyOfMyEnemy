using UnityEngine;
using System.Collections;

public class SpiderSpawner : MonoBehaviour {


    public int enemiesToSpawn;
    public GameObject enemyPrefab;

 

	void Start ()
    {
        StartCoroutine(SpawnEnemy());
	}
	
    IEnumerator SpawnEnemy()
    {
    
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject spiderClone = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
            spiderClone.transform.SetParent(gameObject.transform);
            yield return new WaitForSeconds(2f);
        }

        //Destroy(gameObject);
    }
}
