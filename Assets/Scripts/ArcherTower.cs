using UnityEngine;
using System.Collections;

public class ArcherTower : MonoBehaviour {

    [Header("Put Archer Here")]
    public Rigidbody archerOnTower;

    [Header("Death Properties")]
    public GameObject deathParticles;
    

    void OnDestroy()
    {
        archerOnTower.isKinematic = false;
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
