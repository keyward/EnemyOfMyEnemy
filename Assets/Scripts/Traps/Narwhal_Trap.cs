using UnityEngine;
using System.Collections;

public class Narwhal_Trap : MonoBehaviour
{

	public GameObject narwhal;
    public float invokeSpeed;
    public float stabFrequency;
	private Vector3 spawnPoint = Vector3.zero;
    private MeshRenderer _meshVolume;

    private AudioSource _narwhalSound;


    void Awake()
    {
        _meshVolume = GetComponent<MeshRenderer>();
        _narwhalSound = GetComponent<AudioSource>();
        _meshVolume.enabled = false;
    }

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().MoveSpeedModifier = 0.75f;
			InvokeRepeating ("SpawnHorn", invokeSpeed, stabFrequency);
		}
	}

	void OnCollisionExit (Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().MoveSpeedModifier = 1f;
			CancelInvoke ();
		}
	}

	void OnCollisionStay (Collision col)
	{
        if(col.gameObject.CompareTag("Player"))
		    spawnPoint = col.transform.position;
	}

	private void SpawnHorn ()
	{
		if (this.spawnPoint != Vector3.zero) {
			spawnPoint.Set (spawnPoint.x, -2, spawnPoint.z);
			narwhal.transform.localPosition = spawnPoint;
			Destroy (Instantiate (narwhal), 2);
            _narwhalSound.pitch = Random.Range(.8f, 1.2f);
            _narwhalSound.Play();
		}
		this.spawnPoint = Vector3.zero;
	}
}
