using UnityEngine;
using System.Collections;

public class LarrySoloTrigger : MonoBehaviour {

   

    public GameObject bridge;
    public float bridgeRaiseSpeed;
    public Transform moeTarget;
    private Vector3 bridgeTarget;
    private Moe _moeScript;
    private NavMeshAgent _moeNav;


	void Start ()
    {
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<Moe>();
        _moeNav = GameObject.FindGameObjectWithTag("Moe").GetComponent<NavMeshAgent>();

        bridgeTarget = bridge.transform.position;
        bridge.transform.position += Vector3.down * 10f;
	}


	void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        StartCoroutine(RaiseBridge());
        _moeNav.SetDestination(moeTarget.position);
    }

    IEnumerator RaiseBridge()
    {
        while (Vector3.Distance(bridge.transform.position, bridgeTarget) > .1f)
        {
            bridge.transform.position = Vector3.Lerp(bridge.transform.position, bridgeTarget, Time.deltaTime * bridgeRaiseSpeed);
            yield return null;
        }

        _moeScript.pathFinder.enabled = true;
    }
	
}