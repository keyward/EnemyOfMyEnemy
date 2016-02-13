using UnityEngine;
using System.Collections;

public class DefenderEnemy : AIBaseClass {




    private Transform _moeTransform;
    private bool _navigating;
    [HideInInspector] public bool _shieldActive;
    [HideInInspector] public Health _healthScript;



    protected override void Awake()
    {
        base.Awake();

        _moeTransform = GameObject.FindGameObjectWithTag("Moe").transform;
        _healthScript = GetComponent<Health>();

        _shieldActive = true;
        _healthScript.enabled = false;
    }

	
	
	void Update ()
    {
        if (!_pathFinder.isActiveAndEnabled)
            return;


        if (_shieldActive)
            _pathFinder.SetDestination(_moeTransform.position);
        else
            _pathFinder.SetDestination(_playerTransform.position);
	}
}