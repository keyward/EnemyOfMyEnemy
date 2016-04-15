using UnityEngine;
using System.Collections;

public class DragonFlame : MonoBehaviour
{

	#region Components
	public GameObject fireParticles;
    [Range(0, 45)]
    public float _Angle;
	public float turnSpeed;
	private float _Time;

    public float timeBeforeActivation;
    public float fireCooldown = 4f;
	public bool alwaysOn;

	private GameObject instantiatedFireParticles;
	private bool runCoroutine = true;
    #endregion

    //public GameObject fireDamage;
    //private Color initialColor;
    //private Vector3 startPos;

    void Update ()
	{
		if (this.runCoroutine) {
			StartCoroutine (InstantiateParticle ());
		}
		

		if (this.instantiatedFireParticles != null) {
			_Time = _Time + Time.deltaTime;
			float phase = Mathf.Sin (_Time / turnSpeed);
			instantiatedFireParticles.transform.rotation = Quaternion.Euler (new Vector3 (0, this.transform.rotation.eulerAngles.y + phase * _Angle, 0));
		}
	}

	private IEnumerator InstantiateParticle ()
	{

		this.runCoroutine = false;
        yield return new WaitForSeconds(timeBeforeActivation);

		if (this.instantiatedFireParticles != null) {
			Destroy (this.instantiatedFireParticles);
		} else {
			instantiatedFireParticles = Instantiate (fireParticles, transform.position, Quaternion.Euler (0f, this.transform.rotation.eulerAngles.y, 0f)) as GameObject;
		}
		yield return new WaitForSeconds (fireCooldown);
		if (!this.alwaysOn)
		{
			this.runCoroutine = true;
		}
	}

}
