using UnityEngine;
using System.Collections;

public class MeleeEnemy : MonoBehaviour {


    // Stats
    public int attackPower;
    public float lungeDistance;
    private bool _lunging;
    private bool _stunned;

    // LevelSystem
    private EnemyManager _enemyManagerRef;

    // NavMesh
    private NavMeshAgent _pathFinder;
    private Transform _playerTransform;

    // Audio
    public AudioClip[] meleeSounds;
    private AudioSource _enemyAudio;
    
    // Animations
    private Animator _meleeAnimator;
    private int _attackAnimation;

    public ParticleSystem stunParticles;


    void Awake ()
    {
        _pathFinder = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _enemyAudio = GetComponent<AudioSource>();

        _meleeAnimator = GetComponent<Animator>();
        _attackAnimation = Animator.StringToHash("Attack");

        stunParticles.Stop();

        _lunging = false;
        _stunned = false;

		_pathFinder.stoppingDistance = 5.0f;
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

	void Update ()
    {
        // Melee enemy isn't stunned...
        if(!_stunned)
        {
            // ... and within lunging distance -- lunge //
			if (Vector3.Distance(transform.position, _playerTransform.position) <= lungeDistance)
				StartCoroutine(Lunge());

            // ... and able to move -- go towards player //
            else if (!_lunging)
			{
				_pathFinder.SetDestination(_playerTransform.position);
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
    }
		
}
