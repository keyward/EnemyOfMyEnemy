using UnityEngine;
using System.Collections;

public class FireDamage : MonoBehaviour {

    public int damageAmount = 1;

    void OnParticleCollision(GameObject other)
    {
        print(other.gameObject.name);
        if (other.GetComponent<Health>())
            other.GetComponent<Health>().TakeDamage(damageAmount);
    }
}
