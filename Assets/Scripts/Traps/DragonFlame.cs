using UnityEngine;
using System.Collections;

public class DragonFlame : MonoBehaviour
{

	#region Components
	public GameObject fireDamage;
	public GameObject fireParticles;
	public float _Angle;
	public float _Period;
	private float _Time;
	private Color initialColor;
	private Vector3 startPos;
	private GameObject instantiatedFireParticles;
	private bool runCoroutine = true;
	#endregion
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
	{
		if (this.runCoroutine) {
			StartCoroutine (InstantiateParticle ());
		}
		

		if (this.instantiatedFireParticles != null) {
			_Time = _Time + Time.deltaTime;
			float phase = Mathf.Sin (_Time / _Period);
			instantiatedFireParticles.transform.rotation = Quaternion.Euler (new Vector3 (0, this.transform.rotation.eulerAngles.y + phase * _Angle, 0));
		}
	}

	private IEnumerator InstantiateParticle ()
	{
		this.runCoroutine = false;
		if (this.instantiatedFireParticles != null) {
			Destroy (this.instantiatedFireParticles);
		} else {
			instantiatedFireParticles = Instantiate (fireParticles, transform.position, Quaternion.Euler (0f, this.transform.rotation.eulerAngles.y, 0f)) as GameObject;
		}
		yield return new WaitForSeconds (4);
		this.runCoroutine = true;
	}

	void OnParticleCollision (GameObject other)
	{

	}
}
