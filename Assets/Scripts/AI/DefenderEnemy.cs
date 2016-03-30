using UnityEngine;
using System.Collections;

public class DefenderEnemy : MonoBehaviour {

    [Header("Defender Enemy")]
    public float lungeDistance;

    private Transform _moeTransform;
    [HideInInspector] public bool _shieldActive;
    [HideInInspector] public Health _healthScript;

    private NavMeshAgent _pathFinder;
    private Transform _playerTransform;

    private bool _actionAvailable;
    private bool _lunging;

    private Animator _defenderAnimator;
    private int _attackAnimation;

    public AudioClip[] defenderSounds;
    private AudioSource _defenderAudio;



    void Awake()
    {
        _moeTransform = GameObject.FindGameObjectWithTag("Moe").transform;
        _healthScript = GetComponent<Health>();

        _pathFinder = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _actionAvailable = true;
        _lunging = false;

        _defenderAnimator = GetComponent<Animator>();
        _attackAnimation = Animator.StringToHash("Attack");

        _defenderAudio = GetComponent<AudioSource>();

        _shieldActive = true;
        _healthScript.enabled = false;
    }

	void Update ()
    {
        // if nav mesh is active...
        if (_pathFinder.isActiveAndEnabled)
        {
            // ... and lunging distance -- lunge //
            if (Vector3.Distance(transform.position, _playerTransform.position) <= lungeDistance)
                StartCoroutine(Lunge());

            // ... and Shield is attached -- attack Moe //
            if (_shieldActive && _actionAvailable)
                _pathFinder.SetDestination(_moeTransform.position);

            // ... and Shield has been destroyed -- attack Player //
            else if (!_shieldActive && _actionAvailable)
                _pathFinder.SetDestination(_playerTransform.position);
        }
	}

    IEnumerator Lunge()
    {
        if (_lunging)
            yield break;

        // begin attack animation
        _defenderAnimator.SetTrigger(_attackAnimation);
        _lunging = true;

        _defenderAudio.clip = defenderSounds[0];
        _defenderAudio.Play();

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
        _defenderAnimator.enabled = false;
        yield return new WaitForSeconds(1f);
        _defenderAnimator.enabled = true;

        _lunging = false;
    }
}