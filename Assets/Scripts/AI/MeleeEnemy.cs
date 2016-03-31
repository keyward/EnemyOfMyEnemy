using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeEnemy : MonoBehaviour
{


	// Stats
	public int attackPower;
	public float lungeDistance;
	public float stoppingDistance;
	private bool _lunging;
	private bool _stunned;
	private bool _attacking;
	private bool _retreating;

	private List<MeleeEnemy> enemyList;

	// LevelSystem
	private EnemyManager _enemyManagerRef;

	// NavMesh
	private NavMeshAgent _pathFinder;
	private Transform _playerTransform;
	private Vector3 _retreatPosition;

	// Audio
	public AudioClip[] meleeSounds;
	private AudioSource _enemyAudio;
    
	// Animations
	private Animator _meleeAnimator;
	private int _attackAnimation;

	public ParticleSystem stunParticles;


	void Awake()
	{
		_pathFinder = GetComponent<NavMeshAgent>();
		_playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

		_enemyAudio = GetComponent<AudioSource>();

		_meleeAnimator = GetComponent<Animator>();
		_attackAnimation = Animator.StringToHash("Attack");

		stunParticles.Stop();

		_lunging = false;
		_stunned = false;

		_pathFinder.stoppingDistance = this.stoppingDistance;
	}

	// _enemyManagerRef lets the stage know when the enemy is dead so the player can move on //
	void OnEnable()
	{
		_enemyManagerRef = GameObject.FindGameObjectWithTag("EnemyMgr").GetComponent<EnemyManager>();
	}

	void OnDestroy()
	{
		_enemyManagerRef.RemoveEnemy();
	}
	// ------------------------------------------------------------------------------------- //

	void Update()
	{
		MeleeEnemy[] enemies = GameObject.FindObjectsOfType(typeof(MeleeEnemy)) as MeleeEnemy[];
		this.enemyList = new List<MeleeEnemy>(enemies);
		this.enemyList.Remove((MeleeEnemy)this);


		// Melee enemy isn't stunned...
		if (!_stunned)
		{
			if (!_retreating)
			{
				_pathFinder.SetDestination(_playerTransform.position);

				// Get retreat position
				if (_pathFinder.remainingDistance > this.stoppingDistance)
				{
					this._retreatPosition = this.transform.position;
				}

				// ... and within lunging distance -- lunge //
				if (Vector3.Distance(transform.position, _playerTransform.position) <= lungeDistance)
				{
					StartCoroutine(Lunge());
				}
            // ... and able to move -- go towards player //
			else if (!_lunging && !this._attacking)
				{
					_pathFinder.stoppingDistance = this.stoppingDistance;
					if (!this.checkForAttackers())
					{
						_attacking = true;
					}
				}
				else if (_attacking)
				{
					_pathFinder.stoppingDistance = 1.0f;
				}
			}
			else
			{
				this.retreat();
			}
		}
		else
		{
			if (_meleeAnimator.isActiveAndEnabled)
				_meleeAnimator.enabled = false;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		// shot by player bullet
		if (col.gameObject.CompareTag("Bullet"))
			StartCoroutine(Stun());

        // deal damage to the player //
        else if (col.gameObject.CompareTag("Player"))
			col.gameObject.GetComponent<Health>().TakeDamage(attackPower);
	}

	IEnumerator Stun()
	{
		if (_stunned)
			yield break;

		// stop melee enemy movement/behaviors
		_stunned = true;
		stunParticles.Play();
		_meleeAnimator.enabled = false;
		_pathFinder.Stop();

		// stun noise
		_enemyAudio.clip = meleeSounds[1];
		_enemyAudio.Play();

		yield return new WaitForSeconds(5f);

		// resume melee enemy movement/behaviors        
		stunParticles.Stop();
		_stunned = false;
		_meleeAnimator.enabled = true;
		_pathFinder.Resume();
	}

	IEnumerator Lunge()
	{
		if (_lunging || _stunned)
			yield break;

		// begin attack animation
		_meleeAnimator.SetTrigger(_attackAnimation);
		_lunging = true;

		_enemyAudio.clip = meleeSounds[0];
		_enemyAudio.Play();

		// get players last position to lunge towards
		Vector3 target = _playerTransform.position;
		_pathFinder.SetDestination(target);

		// get chasing speed
		float initialSpeed = _pathFinder.speed;
		float initialAccel = _pathFinder.acceleration;

		// set lunging speed
		_pathFinder.speed = 7f;
		_pathFinder.acceleration = 16f;

		float timeCheck = 0f;
        
		// dash towards player -- attack
		while (Vector3.Distance(transform.position, target) > 2f)
		{
			if (timeCheck >= 2f)
				break;

			timeCheck += Time.deltaTime;
			yield return null;
		}
        
		// set chsing speed
		_pathFinder.speed = initialSpeed;
		_pathFinder.acceleration = initialAccel;

		// pause movement for cooldown
		_meleeAnimator.enabled = false;
		yield return new WaitForSeconds(1f);
		_meleeAnimator.enabled = true;

		_lunging = false;
		_attacking = false;
		this._retreating = true;
	}

	private bool checkForAttackers()
	{
		foreach (MeleeEnemy e in this.enemyList)
		{
			if (e._attacking == true)
			{
				return true;
			}
		}
		return false;
	}

	private void retreat()
	{
		this._pathFinder.SetDestination(this._retreatPosition);
		if (this._pathFinder.remainingDistance < 1.0f)
		{
			this._retreating = false;
		}
		else
		{
			this._retreating = true;
		}
	}
		
}
