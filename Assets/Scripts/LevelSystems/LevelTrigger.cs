using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour {

    [Header("EnemySpawn")]
    public GameObject[] enemySpawners;

    [Header("Spike Walls")]
    public Transform fightBarriers;
    public float raiseSpeed;
    public bool needsToRaiseBarriers;
    public bool onlyNeedLarry;

    public ArchTrigger archTrigger;

    [SerializeField] private int _totalEnemyCount;
    private Transform _enemyManager;
    private MeshRenderer _meshRender;
    private bool triggerActivated;
    private bool _playerCrossed;
    private bool _moeCrossed;

    private bool _roomCleared;

   
    void Awake()
    {
        triggerActivated = false;
        _roomCleared = false;

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


        if (_playerCrossed && _moeCrossed || _playerCrossed && onlyNeedLarry)
        {
            triggerActivated = true;
            _enemyManager.gameObject.SetActive(true);

            Invoke("SpawnEnemies", .5f);

            if (needsToRaiseBarriers)
                StartCoroutine(RaiseBarriers()); 
        }
    }

    public void MinusOneEnemy()
    {
        _totalEnemyCount--;

        if (_totalEnemyCount <= 0 && !_roomCleared)
        {
            StartCoroutine(LowerBarriers());

            if(archTrigger)
                StartCoroutine(archTrigger.LowerBarriers());
        }
           

        Debug.LogWarning(_totalEnemyCount);
    }

    IEnumerator RaiseBarriers()
    {
        Vector3 targetHeight = new Vector3(fightBarriers.transform.position.x, fightBarriers.transform.position.y + 2.7f, fightBarriers.transform.position.z);
        yield return new WaitForSeconds(.5f);

        while (Vector3.Distance(fightBarriers.position, targetHeight) > .25f)
        {
            fightBarriers.position = Vector3.Lerp(fightBarriers.position, targetHeight, Time.deltaTime * raiseSpeed);
            yield return null;
        }
    }

    void SpawnEnemies()
    {
        foreach (GameObject spawner in enemySpawners)
            spawner.SetActive(true);
    }

    IEnumerator LowerBarriers()
    {
        _roomCleared = true;
        while (fightBarriers.position.y > -1)
        {
            fightBarriers.position = Vector3.Lerp(fightBarriers.position, fightBarriers.position + (Vector3.down * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
        
        Destroy(fightBarriers.gameObject);
        Destroy(gameObject);
    }
}
