using UnityEngine;
using System.Collections;

public class OgreBone : MonoBehaviour {

    public float speed;
    private Rigidbody _rb;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 3f);
	}
	
    void Update()
    {
        _rb.transform.Rotate(Vector3.up * Time.deltaTime * 1000);
    }
	
	void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
