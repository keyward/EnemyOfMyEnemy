using UnityEngine;
using System.Collections;

public class MoeNavTargetSystem : MonoBehaviour {

    // get moes navagent
    // when player touches volume - tell moe to go to a certain spot

    public Transform moeTarget;

    private NavMeshAgent _moePathfinder;
    private bool _isActivated;


	void Start ()
    {
        _moePathfinder = GameObject.FindGameObjectWithTag("Moe").GetComponent<NavMeshAgent>();
        _isActivated = false;
	}
	
	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_isActivated)
        {
            _isActivated = true;
            _moePathfinder.SetDestination(moeTarget.position);
        }
    }
}
