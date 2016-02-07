using UnityEngine;
using System.Collections;

public class Narwhal_Trap : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().MoveSpeedModifier *= 0.75f;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().MoveSpeedModifier = 1f;
		}
	}
}
