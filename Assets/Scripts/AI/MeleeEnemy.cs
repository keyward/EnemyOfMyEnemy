using UnityEngine;
using System.Collections;

public class MeleeEnemy : AIBaseClass {


    [Header("Melee Enemy")]
    public int attackPower;
    public float lungeDistance;
	public float circleDistance;
    private EnemyManager _enemyManagerRef;
	private Transform retreatPoint;


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
			if (_actionAvailable) {
				
				if (_attackReady) {
					StartCoroutine (Lunge ());
					print ("lunge");
				}
				// ... and able to move -- go towards player //

				this.Seek ();

			}
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

	protected override void Seek ()
	{
		_pathFinder.SetDestination (_playerTransform.position);
		if (_pathFinder.remainingDistance > this.circleDistance + 1) 
		{
			_pathFinder.Resume ();
			this._attackReady = false;
		} 
		else if (_pathFinder.remainingDistance <= this.circleDistance) 
		{
			_pathFinder.Stop ();
			this._attackReady = true;
		}
		base.Seek ();
	}

	protected override void Retreat ()
	{
		base.Retreat ();
	}
}
