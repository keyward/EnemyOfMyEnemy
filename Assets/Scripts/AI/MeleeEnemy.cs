using UnityEngine;
using System.Collections;

public class MeleeEnemy : AIBaseClass {


    [Header("Melee Enemy")]
    public int attackPower;
    public float lungeDistance;
    private EnemyManager _enemyManagerRef;


    protected override void Awake ()
    {
        base.Awake();
    }

    void OnEnable()
    {
        _enemyManagerRef = GameObject.FindGameObjectWithTag("EnemyMgr").GetComponent<EnemyManager>();
    }
	
    void OnDestroy()
    {
        _enemyManagerRef.RemoveEnemy();
    }

	void Update ()
    {
        // If nav mesh active...
        if(_pathFinder.isActiveAndEnabled)
        {
            // ... and within lunging distance -- lunge //
            if (Vector3.Distance(transform.position, _playerTransform.position) <= lungeDistance)
                StartCoroutine(Lunge());

            // ... and able to move -- go towards player //
            else if (_actionAvailable)
                _pathFinder.SetDestination(_playerTransform.position);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            StartCoroutine(Stun());

        // deal damage to the player //
        else if (col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<Health>().TakeDamage(attackPower);
    }
}
