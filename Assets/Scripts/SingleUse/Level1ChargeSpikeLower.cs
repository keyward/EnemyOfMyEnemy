using UnityEngine;
using System.Collections;

public class Level1ChargeSpikeLower : MonoBehaviour {

    public Transform spikeBarriers;
    private bool _activated;


    void Start()
    {
        _activated = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Moe") && !_activated)
        {
            StartCoroutine(LowerBarriers());
        }
    }

    IEnumerator LowerBarriers()
    {
        Vector3 targetHeight = new Vector3(spikeBarriers.transform.position.x, spikeBarriers.transform.position.y - 3f, spikeBarriers.transform.position.z);

        _activated = true;
        while (Vector3.Distance(spikeBarriers.position, targetHeight) > .25f)
        {
            spikeBarriers.position = Vector3.Lerp(spikeBarriers.position, spikeBarriers.position + (Vector3.down * 3), Time.deltaTime * 3);
            yield return null;
        }
    }
}
