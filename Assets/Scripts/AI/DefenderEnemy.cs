using UnityEngine;
using System.Collections;

public class DefenderEnemy : AIBaseClass {

    [Header("Defender Enemy")]
    public float lungeDistance;

    private Transform _moeTransform;
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
}