using UnityEngine;
using System.Collections;

public class ArchTrigger : MonoBehaviour {

    public Transform moeDestination;
    public Wall_ChargeDestroy breakableWall;

    private PlayerController _playerScript;
    private MoeAI _moeScript;
    private NavMeshAgent _moeNav;
    private MeshRenderer _invisible;

	void Start ()
    {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<MoeAI>();
        _moeNav = GameObject.FindGameObjectWithTag("Moe").GetComponent<NavMeshAgent>();
        _invisible = GetComponent<MeshRenderer>();
        _invisible.enabled = false;

        if (breakableWall)
            breakableWall.canBeDestroyed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerScript.canTaunt = !_playerScript.canTaunt;

            _moeScript.currentState = MoeAI.aiState.stopped;
            _moeNav.Stop();

            if(moeDestination)
                _moeNav.SetDestination(moeDestination.position);

            _moeNav.Resume();

            if (breakableWall)
                breakableWall.canBeDestroyed = true;
        }
    }
}
