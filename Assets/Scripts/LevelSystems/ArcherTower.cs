using UnityEngine;
using System.Collections;

public class ArcherTower : MonoBehaviour {

    [Header("Put Archer Here")]
    public Rigidbody archerOnTower;

    [Header("Death Properties")]
    public GameObject deathParticles;
    private AudioSource _deathSound;

    private Health _towerHealth;

    void Start()
    {
        _deathSound = GetComponent<AudioSource>();
        _towerHealth = GetComponent<Health>();
    }

    void OnDestroy()
    {
        archerOnTower.isKinematic = false;
        _deathSound.Play();
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Bullet"))
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
