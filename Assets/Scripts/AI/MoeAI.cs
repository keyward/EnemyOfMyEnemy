using UnityEngine;
using System.Collections;

public class MoeAI : MonoBehaviour {


    // MoeAI States
    public enum aiState { following, attacking, charging, stoned, stopped };
    
    public aiState currentState;
    [SerializeField] private bool _attacking;
    [SerializeField] private bool _frozen;
    [SerializeField] private bool _idle;
    [SerializeField] private bool _charging;
    private bool _hasAttacked;

    public GameObject attackParticles;

    // Animations
    private Animator _moeAnimator;
    private int _moeAttack;
    private int _moeCharge;
    private int _moeIdle;

    // MoeAI Sounds
    public AudioClip[] moeSounds;
    private AudioSource _moeSoundPlayer;
    // 0 stomp
    // 1 dash
    // 2 stone

    // MoeAI attacks
    private GameObject _areaDamage;
    private GameObject _chargeDamage;

    // MoeAI pathfinding
    private Transform _followTarget;
    private Transform _playerTransform;
    private NavMeshAgent _navAgent;
    private Transform _enemyTarget;

    // Moe "stoned" color change
    public SkinnedMeshRenderer[] moeSkin;
    private Material[] _partsToTurnToStone;


	void Awake ()
    {
        // default states
        currentState = aiState.following;
        _attacking = false;
        _frozen = false;
        _idle = true;
        _charging = false;
        _hasAttacked = false;

        // Moe attack
        _areaDamage = transform.FindChild("AreaAttack").gameObject;
        _chargeDamage = transform.FindChild("FrontAttack").gameObject;
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


        // Getting the materials on Moe's body that can change to stone, and making them invisible
        _partsToTurnToStone = new Material[moeSkin.Length];

        for(int i = 0; i < moeSkin.Length; i++)
        {
            _partsToTurnToStone[i] = moeSkin[i].materials[1];
            _partsToTurnToStone[i].color = new Color(1f, 1f, 1f, 0f);
        }
            
        ChangeState(aiState.following);
	}

    void Update()
    {
        //print(_partsToTurnToStone[0].color.a);

        if(currentState == aiState.following)
            Follow();

        // if moe is told to stop and comes to a complete stop - Idle anim
        else if(!_idle  &&  _navAgent.velocity == Vector3.zero)
        {
            _idle = true;
            _moeAnimator.SetBool(_moeIdle, true);
        }
        else if(_idle && _navAgent.velocity != Vector3.zero)
        {
            _idle = false;
            _moeAnimator.SetBool(_moeIdle, false);
        }

        if (_charging && _frozen)
            if(currentState != aiState.stoned)
                ChangeState(aiState.stoned);
    }
	
    // -- Look at player when idle -- // THIS ONE COULD BE TROUBLE
    void FixedUpdate()
    {
        // if moe is standing still and not doing anything -- look at the player
        if (currentState == aiState.following || currentState == aiState.stopped)
        {
            if(_idle && !_frozen)
            {
                Vector3 targetDirection = _playerTransform.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 2f, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }

    public void ChangeState(aiState moeState)
    {
        //Debug.LogWarning(currentState);

        currentState = moeState;
            
        switch(currentState)
        {
            case aiState.following:
                break;

            case aiState.stoned:
                StartCoroutine(TurnToStone());
                break;

            case aiState.attacking:
                if (_frozen)
                {
                    ChangeState(aiState.stoned);
                    return;
                }
                   

                StartCoroutine(Attack());
                break;

            case aiState.charging:
                StartCoroutine(Charge());
                break;

            case aiState.stopped:
                StopMoe();
                break;

            default:
                ChangeState(aiState.following);
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
            _navAgent.SetDestination(_playerTransform.position);

            if (_navAgent.velocity == Vector3.zero)
                _navAgent.Resume();
        }  
    }

    // -- Area Attack -- //
    IEnumerator Attack()
    {
        if (_attacking || _frozen || _charging)
            yield break;
        
        _attacking = true;

        _navAgent.Stop();
        _navAgent.velocity = Vector3.zero;

        yield return new WaitForSeconds(.5f);

        if (_enemyTarget)
            StartCoroutine(LookAtTarget());

        _moeAnimator.SetTrigger(_moeAttack);

        float attackCheck = 2f;
        

        while(attackCheck > 0)
        {
            if (_hasAttacked)
                yield break;

            attackCheck -= Time.deltaTime;
            yield return null;
        }

        CheckForEnemies();
    }

    // -- Called inside _moeAttack animation event -- //
    public IEnumerator MoeAttack()
    {
        // Attack Effects
        Destroy(Instantiate(attackParticles, transform.position + Vector3.up/1.5f, Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z)), 1f);
        _moeSoundPlayer.clip = moeSounds[0];
        _moeSoundPlayer.Play();

        // Deal damage
        _areaDamage.SetActive(true);
        Time.timeScale = .5f;
        yield return new WaitForSeconds(.1f);
        Time.timeScale = 1.0f;
        _areaDamage.SetActive(false);

        // attack cooldown
        yield return new WaitForSeconds(1f);

        // reset state
        _attacking = false;
        _hasAttacked = true;
        CheckForEnemies();
    }

    // -- Charge at player -- //
    IEnumerator Charge()
    {
        if (_frozen || _charging)
            yield break;

        _charging = true;
        _moeAnimator.SetBool(_moeCharge, true);

        // animation event # 1
        // get initial values
        float normalSpeed = _navAgent.speed;
        float normalAcceleration = _navAgent.acceleration;
        float normalAngularSpeed = _navAgent.angularSpeed;
        float normalStoppingDistance = _navAgent.stoppingDistance;

        // Animation Event
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
        while (_navAgent.remainingDistance > 1f)
        {
            // if stoned in the middle of a charge, told to stop, or taking too long -- cancel the charge
            if (currentState == aiState.stoned || currentState == aiState.stopped || bugCheck >= 4.5f)
                break;

            bugCheck += Time.deltaTime;
            yield return null;
        }
        _moeAnimator.SetBool(_moeCharge, false);

        // reset navAgent values to inital
        _chargeDamage.SetActive(false);
        _navAgent.ResetPath();
        
        _navAgent.speed = normalSpeed;
        _navAgent.acceleration = normalAcceleration;
        _navAgent.angularSpeed = normalAngularSpeed;
        _navAgent.stoppingDistance = normalStoppingDistance;


        // reset ai state
        yield return new WaitForSeconds(.25f);

        if (currentState != aiState.stopped)
            CheckForEnemies();

        // attack cool down
        yield return new WaitForSeconds(1.75f);

        _charging = false;
    }

    // -- Halts Moe's position, and resets attack -- //
    IEnumerator TurnToStone()
    {
        if (_frozen)
            yield break;

        _frozen = true;
        _moeAnimator.enabled = false;

        float initialAngularSpeed = _navAgent.angularSpeed;
        _navAgent.angularSpeed = 0;

        StartCoroutine(StoneColorLerp());

        // stop moving
        _navAgent.SetDestination(transform.position);

        yield return new WaitForSeconds(2f);

        // reset Moe
        _frozen = false;
        _moeAnimator.enabled = true;

        _navAgent.angularSpeed = initialAngularSpeed;

        CheckForEnemies();
    }

    IEnumerator StoneColorLerp()
    {
        if (_frozen)
        {
            // reducing redundant sound effects
            if (_partsToTurnToStone[_partsToTurnToStone.Length - 1].color.a < .25f)
            {
                _moeSoundPlayer.clip = moeSounds[2];
                _moeSoundPlayer.Play();
            }

            // if hes frozen turn him to stone
            while (_partsToTurnToStone[_partsToTurnToStone.Length - 1].color.a <= 1.0f)
            {   
                foreach (Material part in _partsToTurnToStone)
                    part.color += new Color(0f, 0f, 0f, .01f);

                yield return null;
            }
        }
        else
        {
            // if hes not frozen turn him purple again
            while (_partsToTurnToStone[_partsToTurnToStone.Length - 1].color.a > 0.0)
            {
                if (_frozen)
                    break;

                foreach (Material part in _partsToTurnToStone)
                    part.color -= new Color(0f, 0f, 0f, .01f);

                yield return null;
            }
        }
    }

    // -- if Moe is attacking and has an enemy target, rotate towards them -- //
    IEnumerator LookAtTarget()
    {
        while(currentState == aiState.attacking)
        {
            if (currentState == aiState.stoned)
                break;

            if (_enemyTarget)
            {
                Vector3 targetDirection = _enemyTarget.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 3f, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
                break;

            yield return null;
        }
    }

    // Cast a bubble around Moe to see whats there
    void CheckForEnemies()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit[] allHits;
        allHits = Physics.SphereCastAll(ray, 4.5f);

        // if there's anything to react to -- change his state 
        if (allHits.Length > 0)
        {
            foreach (RaycastHit hit in allHits)
            {
                if (hit.collider.CompareTag("Fear"))
                {
                    ChangeState(aiState.stoned);
                    break;
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    ChangeState(aiState.attacking);
                    break;
                }
                else
                {
                    ChangeState(aiState.following);
                    _frozen = false;
                    StartCoroutine(StoneColorLerp());
                }
            }
        }
    }

    public void StopMoe()
    {
        _moeAnimator.SetBool(_moeCharge, false);
        _moeAnimator.SetBool(_moeIdle, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_frozen)
            return;

        // Touch a pixie to turn him to stone
        if (other.CompareTag("Fear"))
            ChangeState(aiState.stoned);

        // if Moe has not been taunted or touched by a pixie -- he will attack
        else if (other.CompareTag("Enemy"))
        {
            if (_frozen)
                return;


            if (currentState == aiState.charging)
                return;
            else if (currentState == aiState.stoned)
                return;

            if (!_enemyTarget)
                _enemyTarget = other.transform;

            ChangeState(aiState.attacking);
        }
    }
}