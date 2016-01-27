using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


    public float speed;
    public GameObject deathParticles;

    private Rigidbody rb;
    private AudioSource bulletSound;
    
    void Awake()
    {
        bulletSound = GetComponent<AudioSource>();
        bulletSound.pitch = Random.Range(.9f, 1.3f);
        bulletSound.Play();
    }

	void Start ()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter( Collision col )
    {
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        
        if (col.gameObject.CompareTag("Destructible"))
            col.gameObject.GetComponent<Health>().TakeDamage(2);

        Destroy(gameObject);
    }
}
