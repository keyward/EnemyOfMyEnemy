using UnityEngine;
using System.Collections;

public class Narwhal_Trap : MonoBehaviour
{

	public GameObject narwhal;
	private Vector3 spawnPoint = Vector3.zero;

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().MoveSpeedModifier = 0.75f;
			InvokeRepeating ("SpawnHorn", 2, 5);
		}
	}

	void OnCollisionExit (Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().MoveSpeedModifier = 1f;
			CancelInvoke();
		}
	}

	void OnCollisionStay (Collision col)
	{
		spawnPoint = col.transform.position;
	}

	private void SpawnHorn ()
	{
		if (this.spawnPoint != Vector3.zero) {
			spawnPoint.Set(spawnPoint.x,0,spawnPoint.z);
			narwhal.transform.localPosition = spawnPoint;
			Destroy (Instantiate (narwhal), 3);
		}
		this.spawnPoint = Vector3.zero;
	}
}
