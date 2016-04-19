using UnityEngine;
using System.Collections;

public class TauntDisable : MonoBehaviour {


    public Transform spikeWall;
    public float raiseSpeed;

    private MeshRenderer _render;
    private bool _spikesUp;

	void Start ()
    {
        _spikesUp = false;

        _render = GetComponent<MeshRenderer>();
        _render.enabled = false;
	}
	
	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //print("touched taunt disabler");
            StartCoroutine(RaiseSpikes());
        }
    }

    IEnumerator RaiseSpikes()
    {
        if (_spikesUp || !spikeWall)
            yield break;

        _spikesUp = true;

        Vector3 targetHeight = new Vector3(spikeWall.transform.position.x, spikeWall.transform.position.y + 2.7f, spikeWall.transform.position.z);

        while (Vector3.Distance(spikeWall.position, targetHeight) > .25f)
        {
            spikeWall.position = Vector3.Lerp(spikeWall.position, targetHeight, Time.deltaTime * raiseSpeed);
            yield return null;
        }
    }
}
