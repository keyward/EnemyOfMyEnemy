using UnityEngine;
using System.Collections;

public class Ringout : MonoBehaviour {

    public Transform resetPosition;

	

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.gameObject.transform.position = resetPosition.position;
    }
}
