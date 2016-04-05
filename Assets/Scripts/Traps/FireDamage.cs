using UnityEngine;
using System.Collections;

public class FireDamage : MonoBehaviour {

    public int damageAmount = 1;

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Health>().TakeDamage(damageAmount);
    }
}
