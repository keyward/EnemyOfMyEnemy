using UnityEngine;
using System.Collections;

public class ArchTrigger : MonoBehaviour {

    public Transform moeDestination;
    private Moe _moeScript;
    private NavMeshAgent _moeNav;

	void Start ()
    {
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<Moe>();
        _moeNav = GameObject.FindGameObjectWithTag("Moe").GetComponent<NavMeshAgent>();
	}
	
	
	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _moeScript.StopCoroutine("FollowCheck");
            _moeScript._isFollowing = false;
            _moeNav.Stop();

            if(moeDestination)
                _moeNav.SetDestination(moeDestination.position);

            _moeNav.Resume();
            print("player through arches");
            print(_moeNav.destination);
        }
    }
}
