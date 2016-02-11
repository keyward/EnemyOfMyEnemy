using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour {

    [Header("DRAG SPAWNERS HERE")]
    public GameObject[] enemySpawners;

    [Header("DRAG BARRIER OBJECTS HERE")]
    public Transform fightBarriers;

    public float raiseSpeed;
    public int totalEnemyCount;


    private Transform _enemyManager;
    private bool triggerActivated;
    private bool _playerCrossed;
    private bool _moeCrossed;

   
    
    void Awake()
    {
        triggerActivated = false;

        foreach(GameObject spawner in enemySpawners)
        {
            spawner.SetActive(false);
            totalEnemyCount += spawner.GetComponent<EnemySpawner>().enemiesToSpawn;
        }

        _enemyManager = transform.FindChild("EnemyManager");
    }

    void OnTriggerEnter(Collider other)
    {
        if(triggerActivated)
            return;

        if (other.CompareTag("Player"))
            _playerCrossed = true;
        else if(other.CompareTag("Moe"))
            _moeCrossed = true;



        if (_playerCrossed && _moeCrossed)
        {
            triggerActivated = true;

            foreach (GameObject spawner in enemySpawners)
                spawner.SetActive(true);

            StartCoroutine(RaiseBarriers());
        }
    }

    public void MinusOneEnemy()
    {
        totalEnemyCount--;

        if (totalEnemyCount <= 0)
            StartCoroutine(LowerBarriers());

        print(totalEnemyCount);
    }

    IEnumerator RaiseBarriers()
    {
        yield return new WaitForSeconds(1f);

        while(fightBarriers.position.y < 2)
        {
            fightBarriers.position = Vector3.Lerp(fightBarriers.position, fightBarriers.position + (Vector3.up * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
    }

    IEnumerator LowerBarriers()
    {
        while (fightBarriers.position.y > -1)
        {
            fightBarriers.position = Vector3.Lerp(fightBarriers.position, fightBarriers.position + (Vector3.down * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
        Destroy(fightBarriers.gameObject);
        Destroy(gameObject);
    }
}
