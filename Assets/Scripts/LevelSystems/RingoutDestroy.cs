using UnityEngine;
using System.Collections;

public class RingoutDestroy : MonoBehaviour {

	
	void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
            Destroy(col.gameObject);
    }
}
