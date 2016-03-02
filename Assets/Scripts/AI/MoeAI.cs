using UnityEngine;
using System.Collections;

public class MoeAI : MonoBehaviour {


    // MoeAI States
    public enum aiState { following, attacking, charging, stoned, stopped };
    [HideInInspector] public aiState currentState;
    private bool _attacking;
    private bool _frozen;

    public GameObject attackParticles;

    // MoeAI Sounds
    public AudioClip[] moeSounds;
    private AudioSource _moeSoundPlayer;

    // MoeAI attacks
    private GameObject _areaDamage;
    private GameObject _chargeDamage;

    // MoeAI pathfinding
    private Transform _playerTransform;
    private Vector3 _lastPlayerLocation;
    private NavMeshAgent _navAgent;

    // color conveyance
    private Renderer _render;
    

	void Awake ()
    {
        // default states
        currentState = aiState.following;
        _attacking = false;
        _frozen = false;

        // Moe attack
        _areaDamage = transform.GetChild(0).gameObject;
        _chargeDamage = transform.GetChild(1).gameObject;
        _areaDamage.SetActive(false);
        _chargeDamage.SetActive(false);

        // navigation
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navAgent = GetComponent<NavMeshAgent>();

        // sounds 
        _moeSoundPlayer = GetComponent<AudioSource>();

        // ************************************* //
        // ** Drives the entire State Machine ** //
        // ************************************* //
        InvokeRepeating("StateLogic", 0f, .1f);


        _render = GetComponent<Renderer>();
	}
	
    void StateLogic()
    {
        //Debug.LogWarning(currentState);

        if (currentState == aiState.stopped)
            return;

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
        _render.material.color = Color.cyan;

        if (_navAgent.velocity != Vector3.zero)
            _navAgent.Stop();

        yield return new WaitForSeconds(.5f);

        if(currentState != aiState.stoned)
        {
            _moeSoundPlayer.clip = moeSounds[0];
            _moeSoundPlayer.Play();
            // play animation "Attack"
            Destroy(Instantiate(attackParticles, transform.position, Quaternion.identity), 1f);
            _areaDamage.SetActive(true);
        }
        
        yield return new WaitForSeconds(.1f);

        _areaDamage.SetActive(false);

        // reset state
        currentState = aiState.following;
        _render.material.color = Color.green;

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
        _render.material.color = Color.red;

        // set charge speed - Charge
        _navAgent.speed = 50f;
        _navAgent.acceleration = 100f;
        _navAgent.angularSpeed = 360f;
        _navAgent.stoppingDistance = 0f;
        _navAgent.Resume();

        // audio 
        _moeSoundPlayer.clip = moeSounds[1];
        _moeSoundPlayer.Play();

        // charge at players last position
        while(_navAgent.remainingDistance > .01f)
        {
            // if stoned in the middle of a charge -- cancel the charge
            if (currentState == aiState.stoned)
                break;
 
            // perform charge
            yield return null;
        }

        // reset navAgent values to inital
        _chargeDamage.SetActive(false);
        _navAgent.ResetPath();
        _navAgent.speed = normalSpeed;
        _navAgent.acceleration = normalAcceleration;
        _navAgent.angularSpeed = normalAngularSpeed;
        _navAgent.stoppingDistance = normalStoppingDistance;

        yield return new WaitForSeconds(.75f);

        // reset ai state
        currentState = aiState.following;
        
        // attack cool down
        yield return new WaitForSeconds(3.5f);

        _render.material.color = Color.green;
        _attacking = false;
    }

    // -- Halts Moe's position, and resets attack -- //
    IEnumerator TurnToStone()
    {
        if (_frozen)
            yield break;

        _frozen = true;
        // swap texture to stone texture
        _render.material.color = Color.grey;

        // stop moving
        _navAgent.velocity = Vector3.zero;
        _navAgent.Stop();

        yield return new WaitForSeconds(1f);

        // swap texture to initial texture
        // continue following player


        if (currentState != aiState.stoned)
            _render.material.color = Color.green;

        // reset Moe
        currentState = aiState.following;
        _frozen = false;
    }

    void OnTriggerStay(Collider other)
    {
        // as long as Moe is touching a pixie he will remain as stone
        if (other.CompareTag("Fear"))
            currentState = aiState.stoned;
        
        // if Moe has not been taunted or touched by a pixie -- he will attack
        else if (other.CompareTag("Enemy") && currentState != aiState.stoned && currentState != aiState.charging)
            currentState = aiState.attacking;
    }
}