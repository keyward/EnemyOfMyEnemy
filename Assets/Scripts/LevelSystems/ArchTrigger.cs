using UnityEngine;
using System.Collections;

public class ArchTrigger : MonoBehaviour {

    public Transform moeDestination;
    public Wall_ChargeDestroy breakableWall;
    public Transform spikeWall;
    public float raiseSpeed = 2.5f;

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
            //_moeNav.Stop();
            //_moeNav.velocity = Vector3.zero;

            if(moeDestination)
            {
                _moeNav.SetDestination(moeDestination.position);
                //_moeNav.Resume();
            }

            if (breakableWall)
                breakableWall.canBeDestroyed = true;
        }
    }

    public IEnumerator LowerBarriers()
    { 
        while (spikeWall.position.y > -1)
        {
            spikeWall.position = Vector3.Lerp(spikeWall.position, spikeWall.position + (Vector3.down * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
    }
}
