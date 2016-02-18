using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour {

    [Header("DRAG SPAWNERS HERE")]
    public GameObject[] enemySpawners;

    [Header("DRAG BARRIER OBJECTS HERE")]
    public Transform fightBarriers;

    public float raiseSpeed;

    [SerializeField] private int _totalEnemyCount;
    private Transform _enemyManager;
    private MeshRenderer _meshRender;
    private bool triggerActivated;
    private bool _playerCrossed;
    private bool _moeCrossed;

   
    
    void Awake()
    {
        triggerActivated = false;

        foreach(GameObject spawner in enemySpawners)
        {
            spawner.SetActive(false);
            _totalEnemyCount += spawner.GetComponent<EnemySpawner>().enemiesToSpawn;
        }

        _enemyManager = transform.FindChild("EnemyManager");
        _enemyManager.gameObject.SetActive(false);

        _meshRender = GetComponent<MeshRenderer>();
        _meshRender.enabled = false;
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
            _enemyManager.gameObject.SetActive(true);
        }
    }

    public void MinusOneEnemy()
    {
        _totalEnemyCount--;

        if (_totalEnemyCount <= 0)
            StartCoroutine(LowerBarriers());

        print(_totalEnemyCount);
    }

    IEnumerator RaiseBarriers()
    {
        print("raising barriers");
        yield return new WaitForSeconds(.5f);

        for(float i = 0; i < 4; i += .2f)
        {
            fightBarriers.position = Vector3.Lerp(fightBarriers.position, fightBarriers.position + (Vector3.up * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
    }

    IEnumerator LowerBarriers()
    {
        print("lower barriers");
        while (fightBarriers.position.y > -1)
        {
            fightBarriers.position = Vector3.Lerp(fightBarriers.position, fightBarriers.position + (Vector3.down * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
        Destroy(fightBarriers.gameObject);
        Destroy(gameObject);
    }
}
