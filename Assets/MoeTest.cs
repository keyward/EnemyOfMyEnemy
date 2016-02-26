using UnityEngine;
using System.Collections;

public class MoeTest : MonoBehaviour {


    enum aiState { following, attacking, charging, stoned };

    aiState currentState;

    private Transform _playerTransform;
    private Vector3 _lastPlayerLocation;
    private NavMeshAgent _navAgent;

    private bool _attacking;

	void Awake ()
    {
        currentState = aiState.following;

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navAgent = GetComponent<NavMeshAgent>();


        _attacking = false;

        InvokeRepeating("StateLogic", 0f, .1f);
	}
	
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
            currentState = aiState.attacking;

        else if (Input.GetKeyDown(KeyCode.S))
            currentState = aiState.charging;
	}

    void StateLogic()
    {
        //print("checking states");
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
                TurnToStone();
                break;
            default:
                print("whatever");
                break;
        }
    }

    void Follow()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > 5f)
        {
            if(_navAgent.velocity == Vector3.zero)
                _navAgent.Resume();

            _navAgent.SetDestination(_playerTransform.position);
        }  
        else
            _navAgent.Stop();

        //print("following");
    }

    IEnumerator Attack()
    {
        if (_attacking)
            yield break;

        _attacking = true;

        if (_navAgent.velocity != Vector3.zero)
            _navAgent.Stop();

        print("attacking");

        yield return new WaitForSeconds(1f);

        currentState = aiState.following;
        _attacking = false;
    }

    IEnumerator Charge()
    {
        if (_attacking)
            yield break;

        _attacking = true;
        print("starting charge");


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

        //set charge speed
        _navAgent.speed = 50f;
        _navAgent.acceleration = 100f;
        _navAgent.angularSpeed = 360f;
        _navAgent.stoppingDistance = 0f;
        _navAgent.Resume();

        // charge at players last position
        while(_navAgent.remainingDistance > .01f)
            yield return null;
        
        // reset navAgent values to inital
        _navAgent.ResetPath();
        _navAgent.speed = normalSpeed;
        _navAgent.acceleration = normalAcceleration;
        _navAgent.angularSpeed = normalAngularSpeed;
        _navAgent.stoppingDistance = normalStoppingDistance;

        yield return new WaitForSeconds(.75f);
        print("charge over");

        // reset ai state
        _attacking = false;
        currentState = aiState.following;
    }

    void TurnToStone()
    {
        print("stoned");
        // stop moving for a second, if charging stopDestination
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fear"))
            currentState = aiState.stoned;
    }

}