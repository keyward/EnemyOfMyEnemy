using UnityEngine;
using System.Collections;



public class MoeAI : MonoBehaviour {


    // MoeAI States
    public enum aiState { following, attacking, charging, stoned, stopped };
    [HideInInspector] public aiState currentState;
    private bool _attacking;
    private bool _frozen;
    private bool _idle;

    // Animations - Particles
    public GameObject attackParticles;
    private Animator _moeAnimator;
    private int _moeAttack;
    private int _moeCharge;
    private int _moeIdle;

    // MoeAI Sounds
    public AudioClip[] moeSounds;
    private AudioSource _moeSoundPlayer;
    // 0 stomp
    // 1 dash

    // MoeAI attacks
    private GameObject _areaDamage;
    private GameObject _chargeDamage;

    // MoeAI pathfinding
    private Transform _playerTransform;
    private Vector3 _lastPlayerLocation;
    private NavMeshAgent _navAgent;
    private Transform _enemyTarget;

    // Moe "stoned" color change
    public SkinnedMeshRenderer _skinMesh;
    private Color32 _stoneColor;
    

	void Awake ()
    {
        // default states
        currentState = aiState.following;
        _attacking = false;
        _frozen = false;
        _idle = true;

        // Moe attack
        _areaDamage = transform.GetChild(0).gameObject;
        _chargeDamage = transform.GetChild(1).gameObject;
        _areaDamage.SetActive(false);
        _chargeDamage.SetActive(false);

        // navigation
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navAgent = GetComponent<NavMeshAgent>();

        // etc Components
        _moeSoundPlayer = GetComponent<AudioSource>();
        _moeAnimator = GetComponent<Animator>();
        _moeAttack = Animator.StringToHash("Attack");
        _moeCharge = Animator.StringToHash("Charging");
        _moeIdle = Animator.StringToHash("Idling");

        // ************************************* //
        // ** Drives the entire State Machine ** //
        // ************************************* //
        InvokeRepeating("StateLogic", 0f, .1f);
	}
	
    void FixedUpdate()
    {
        // slowly rotate Moe towards player if he is standing still and can move
        if (_idle && !_frozen)
        {
            Vector3 targetDirection = _playerTransform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 2f, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    void StateLogic()
    {
        //Debug.LogWarning(currentState);

        if (currentState == aiState.stopped)
        {
            // if moe has reached his destination - stop walking
            if(_navAgent.velocity == Vector3.zero && !_idle)
            {
                _idle = true;
                _moeAnimator.SetBool(_moeIdle, true);
            }

            // if moe is stopped don't do anything
            return;
        }
            

        switch(currentState)
        {
            case aiState.following:
                Follow();
                break;

            case aiState.stoned:
                StartCoroutine(TurnToStone());
                break;

            case aiState.attacking:
                StartCoroutine(Attack());
                break;

            case aiState.charging:
                StartCoroutine(Charge());
                break;

            case aiState.stopped:
                break;

            default:
                Follow();
                break;
        }
    }

    void Follow()
    {
        // if moe is following the player and isn't moving - idle
        if (_navAgent.velocity == Vector3.zero && !_idle)
        {
            _idle = true;
            _moeAnimator.SetBool(_moeIdle, true);
        }
        // if moe is following and is moving - run
        else if (_navAgent.velocity != Vector3.zero && _idle)
        {
            _idle = false;
            _moeAnimator.SetBool(_moeIdle, false);
        }

        // stops 5 meters from player //
        if (Vector3.Distance(transform.position, _playerTransform.position) > 6f)
        {
            if(_navAgent.velocity == Vector3.zero)
                _navAgent.Resume();

            _navAgent.SetDestination(_playerTransform.position);
        }  
        else
            _navAgent.Stop();
    }

    // -- Area Attack -- //
    IEnumerator Attack()
    {
        if (_attacking || _frozen)
            yield break;

        _attacking = true;

        if (_navAgent.velocity != Vector3.zero)
            _navAgent.Stop();

        yield return new WaitForSeconds(.5f);

        if(currentState != aiState.stoned)
        {
            if (_enemyTarget)
                StartCoroutine(LookAtTarget());

            _moeAnimator.SetTrigger(_moeAttack);
        }
    }

    // -- Called inside _moeAttack animation event -- //
    public IEnumerator MoeAttack()
    {
        // Attack Effects
        Destroy(Instantiate(attackParticles, transform.position, Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z)), 1f);
        _moeSoundPlayer.clip = moeSounds[0];
        _moeSoundPlayer.Play();

        // Deal damage
        _areaDamage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _areaDamage.SetActive(false);

        // reset state
        currentState = aiState.following;

        // attack cooldown
        yield return new WaitForSeconds(1f);

        _attacking = false;
    }

    // -- Charge at player -- //
    IEnumerator Charge()
    {
        if (_attacking || _frozen)
            yield break;

        _attacking = true;
        _moeAnimator.SetBool(_moeCharge, true);
        
        // get initial values
        float normalSpeed = _navAgent.speed;
        float normalAcceleration = _navAgent.acceleration;
        float normalAngularSpeed = _navAgent.angularSpeed;
        float normalStoppingDistance = _navAgent.stoppingDistance;

        // get players last position
        Vector3 target = _playerTransform.position;
        _navAgent.Stop();
        _navAgent.SetDestination(target);

        yield return new WaitForSeconds(.2f);

        _chargeDamage.SetActive(true);

        // set charge speed - Charge
        _navAgent.speed = 10f;
        _navAgent.acceleration = 16f;
        _navAgent.angularSpeed = 360f;
        _navAgent.stoppingDistance = 0f;
        _navAgent.Resume();

        // audio 
        _moeSoundPlayer.clip = moeSounds[1];
        _moeSoundPlayer.Play();

        float bugCheck = 0f;

        // charge at players last position
        while(_navAgent.remainingDistance > .5f)
        {
            // if stoned in the middle of a charge -- cancel the charge
            if (currentState == aiState.stoned || currentState == aiState.stopped || bugCheck >= 3f)
                break;

            bugCheck += Time.deltaTime;

            // perform charge
            yield return null;
        }
        

        // reset navAgent values to inital
        _chargeDamage.SetActive(false);
        _navAgent.ResetPath();
        _moeAnimator.SetBool(_moeCharge, false);
        _navAgent.speed = normalSpeed;
        _navAgent.acceleration = normalAcceleration;
        _navAgent.angularSpeed = normalAngularSpeed;
        _navAgent.stoppingDistance = normalStoppingDistance;

        
        // until we have some sort of conveyance for Moe stopping - this needs to stay commented
        //yield return new WaitForSeconds(.75f);

        // reset ai state
        if(currentState != aiState.stopped)
            currentState = aiState.following;
        
        // attack cool down
        yield return new WaitForSeconds(3.5f);

        _attacking = false;
    }

    // -- Halts Moe's position, and resets attack -- //
    IEnumerator TurnToStone()
    {
        if (_frozen)
            yield break;

        _frozen = true;
        _moeAnimator.enabled = false;
        StartCoroutine(StoneColorLerp());

        // stop moving
        _navAgent.velocity = Vector3.zero;
        _navAgent.Stop();

        yield return new WaitForSeconds(2f);

        // reset Moe
        currentState = aiState.following;
        _frozen = false;
        _moeAnimator.enabled = true;
        StartCoroutine(StoneColorLerp());
    }

    IEnumerator StoneColorLerp()
    {
        // if hes frozen make his skin black
        if(_frozen)
            while(_skinMesh.material.color.r >= .49f)
            {
                // Texture Lerp to stone texture
                _skinMesh.material.color -= new Color(.01f, .01f, .01f);
                yield return null;
            }
        // if he's unfrozen change it back to standard
        else
            while(_skinMesh.material.color.r < 1.0f)
            {
                // Texture Lerp back to regular texture
                _skinMesh.material.color += new Color(.01f, .01f, .01f);
                yield return null;
            }
    }

    IEnumerator LookAtTarget()
    {
        while(currentState == aiState.attacking)
        {
            // if moe isn't frozen rotate towards enemy
            if(!_frozen && _enemyTarget)
            {
                Vector3 targetDirection = _enemyTarget.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 2f, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

            yield return null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // as long as Moe is touching a pixie he will remain as stone
        if (other.CompareTag("Fear"))
            currentState = aiState.stoned;
        
        // if Moe has not been taunted or touched by a pixie -- he will attack
        else if (other.CompareTag("Enemy") && currentState != aiState.stoned && currentState != aiState.charging)
        {
            if (!_enemyTarget)
                _enemyTarget = other.transform;

            currentState = aiState.attacking;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fear"))
            print("unfreeze");

        if (other.CompareTag("Enemy"))
            print("stop attacking");
    }
}