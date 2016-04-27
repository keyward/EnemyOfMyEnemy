using UnityEngine;
using System.Collections;

public class ArchTrigger : MonoBehaviour {

    public Transform moeDestination;
    public Wall_ChargeDestroy breakableWall;
    public Transform spikeWall;
    public float raiseSpeed = 2.5f;
    public bool disableTaunt;

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
            _playerScript.tauntDisabled = disableTaunt;

            _moeScript.ChangeState(MoeAI.aiState.stopped);

            if(moeDestination)
                _moeNav.SetDestination(moeDestination.position);
            
                
            if (breakableWall)
                breakableWall.canBeDestroyed = true;
        }
    }

    public IEnumerator LowerBarriers()
    {
        Vector3 targetHeight = new Vector3(spikeWall.transform.position.x, spikeWall.transform.position.y - 3f, spikeWall.transform.position.z);

        while (Vector3.Distance(spikeWall.position, targetHeight) > .25f)
        {
            spikeWall.position = Vector3.Lerp(spikeWall.position, spikeWall.position + (Vector3.down * 3), Time.deltaTime * raiseSpeed);
            yield return null;
        }
    }


    public void LowerSpikesForBarrierDestroy()
    {
        StartCoroutine(LowerBarriers());
    }
}
