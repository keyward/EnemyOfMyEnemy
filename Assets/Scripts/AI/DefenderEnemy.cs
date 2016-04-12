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
    private bool _moving;
    private bool _paused;

    public Animator defenderAnimator;
    private AnimatorStateInfo currentBaseState;
    private int _attackAnimation;
    private int _movingAnimationState;
    public int shieldBreakTrigger;
    public int _shieldActiveBool;

    public AudioClip[] defenderSounds;
    private AudioSource _defenderAudio;

    public float distance;

    void Awake()
    {
        _moeTransform = GameObject.FindGameObjectWithTag("Moe").GetComponent<Transform>();
        _healthScript = GetComponent<Health>();

        _pathFinder = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _lunging = false;
        _moving = false;
        _paused = false;

        defenderAnimator = GetComponent<Animator>();
        _attackAnimation = Animator.StringToHash("Attack");
        _movingAnimationState = Animator.StringToHash("Moving");
        shieldBreakTrigger = Animator.StringToHash("ShieldBreak");
        _shieldActiveBool = Animator.StringToHash("ShieldActive");

        defenderAnimator.SetBool(_shieldActiveBool, true);

        _defenderAudio = GetComponent<AudioSource>();

        _shieldActive = true;
        _healthScript.enabled = false;
    }

	void Update ()
    { 
        if (_pathFinder.velocity == Vector3.zero)
            defenderAnimator.SetBool(_movingAnimationState, false);
        
        else if(_pathFinder.velocity != Vector3.zero)
            defenderAnimator.SetBool(_movingAnimationState, true);
        

        if (_paused)
            return;
        
        if(_pathFinder.isActiveAndEnabled)
        {
            // ... and Shield is attached -- attack Moe //
            if (_shieldActive)
            {
                if (Vector3.Distance(transform.position, _playerTransform.position) > 5f)
                    _pathFinder.SetDestination(_moeTransform.position);
                else
                    _pathFinder.SetDestination(transform.position);
            }
            else if (!_shieldActive)
            {
                if (Vector3.Distance(transform.position, _playerTransform.position) <= lungeDistance && !_lunging)
                    StartCoroutine(Lunge());

                else if(!_lunging)
                    _pathFinder.SetDestination(_playerTransform.position);
            }
        }
    }

    public void ShieldBreak()
    {
        StartCoroutine(BrokenShieldReaction());

        gameObject.tag = "Enemy";

        _paused = true;

        _shieldActive = false;
        _healthScript.enabled = true;
        defenderAnimator.SetTrigger(shieldBreakTrigger);
        defenderAnimator.SetBool(_shieldActiveBool, false);


        _paused = false;
    }

    IEnumerator BrokenShieldReaction()
    {
        _pathFinder.enabled = false;

        yield return new WaitForSeconds(1f);

        _pathFinder.enabled = true;
    }

    IEnumerator Lunge()
    {
       // if (_lunging)
           // yield break;

        _lunging = true;

        // begin attack animation
        defenderAnimator.SetTrigger(_attackAnimation);
        

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
        _pathFinder.enabled = false;

        // set chsing speed
        _pathFinder.speed = initialSpeed;
        _pathFinder.acceleration = initialAccel;

        yield return new WaitForSeconds(.5f);
        _pathFinder.enabled = true;

        _lunging = false;
    }
}