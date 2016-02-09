using UnityEngine;
using System.Collections;

public class DefenderEnemy : MonoBehaviour {


    /*
        Pursue Moe and bump into him
        Nothing can hurt him with shield 
        WeakSpot in the back that destroys the shield
        after shield is gone, increase speed, target larry, and engage


    */

    private bool _navigating;
    public bool _shieldActive;
    private NavMeshAgent _pathFinder;
    private Transform _moeTransform;
    private Transform _playerTransform;
    public Health _healthScript;



    void Awake()
    {
        _pathFinder = GetComponent<NavMeshAgent>();
        _moeTransform = GameObject.FindGameObjectWithTag("Moe").transform;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _navigating = true;
        _shieldActive = true;

        _healthScript = GetComponent<Health>();
        _healthScript.enabled = false;
    }

	
	
	void Update ()
    {
	    if(_navigating && _pathFinder.isActiveAndEnabled)
        {
            if (_shieldActive)
                _pathFinder.SetDestination(_moeTransform.position);
            else
                _pathFinder.SetDestination(_playerTransform.position);
        }
	}
}