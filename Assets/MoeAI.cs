using UnityEngine;
using System.Collections;

public class MoeAI : MonoBehaviour {


    enum aiState { following, attacking, charging, stoned };
    aiState currentState;


    // MoeAI attacks
    private GameObject _areaDamage;
    private GameObject _chargeDamage;

    // MoeAI pathfinding
    private Transform _playerTransform;
    private Vector3 _lastPlayerLocation;
    private NavMeshAgent _navAgent;

    private bool _attacking;
    private bool _frozen;

	void Awake ()
    {
        currentState = aiState.following;

        _areaDamage = transform.GetChild(0).gameObject;
        _chargeDamage = transform.GetChild(1).gameObject;
        _areaDamage.SetActive(false);
        _chargeDamage.SetActive(false);


        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navAgent = GetComponent<NavMeshAgent>();

        _attacking = false;
        _frozen = false;

        // -- Drives the entire State Machine -- //
        InvokeRepeating("StateLogic", 0f, .1f);
	}
	
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
            currentState = aiState.attacking;

        else if (Input.GetKeyDown(KeyCode.S))
            currentState = aiState.charging;

        else if (Input.GetKeyDown(KeyCode.D))
            currentState = aiState.stoned;
	}

    void StateLogic()
    {
        switch(currentState)
        {
            case aiState.following:
                Follow();
                break;

            case aiState.attacking:
                StartCoroutine(Attack());
                break;

            case aiState.charging:
                StartCoroutine(Charge());
                break;

            case aiState.stoned:
                StartCoroutine(TurnToStone());
                break;

            default:
                Follow();
                break;
        }
    }

    void Follow()
    {
        // stops 5 meters from player //
        if (Vector3.Distance(transform.position, _playerTransform.position) > 5f)
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

        print("attacking");
        _areaDamage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _areaDamage.SetActive(false);
        // play animation "Attack"
        // Instantiate particles

        yield return new WaitForSeconds(1f);

        currentState = aiState.following;
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

        //set charge speed
        _navAgent.speed = 50f;
        _navAgent.acceleration = 100f;
        _navAgent.angularSpeed = 360f;
        _navAgent.stoppingDistance = 0f;
        _navAgent.Resume();

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
        _attacking = false;
        currentState = aiState.following;
    }

    // -- Halts Moe's position, and resets attack -- //
    IEnumerator TurnToStone()
    {
        if (_frozen)
            yield break;

        _frozen = true;

        // swap texture to stone texture
        _navAgent.velocity = Vector3.zero;
        _navAgent.Stop();

        yield return new WaitForSeconds(1f);

        // swap texture to initial texture
        // continue following player
        currentState = aiState.following;
        _frozen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fear"))
            currentState = aiState.stoned;
    }

}