using UnityEngine;
using System.Collections;

public class DefenderEnemy : MonoBehaviour {




    [HideInInspector] public bool _shieldActive;
    [HideInInspector] public Health _healthScript;

    private NavMeshAgent _pathFinder;
    private Transform _moeTransform;
    private Transform _playerTransform;
  

    void Awake()
    {
        _pathFinder = GetComponent<NavMeshAgent>();
        _moeTransform = GameObject.FindGameObjectWithTag("Moe").transform;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _shieldActive = true;

        _healthScript = GetComponent<Health>();
        _healthScript.enabled = false;
    }

	
	void Update ()
    {
	    if(_pathFinder.isActiveAndEnabled)
        {
            if (_shieldActive)
                _pathFinder.SetDestination(_moeTransform.position);
            else
                _pathFinder.SetDestination(_playerTransform.position);
        }
	}
}