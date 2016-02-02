using UnityEngine;
using System.Collections;

public class AreaDamage : MonoBehaviour {


    public float explosionForce;
    public float explosionRadius;
    public int attackPower;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(attackPower);

            if(other.gameObject.GetComponent<Rigidbody>())
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        else if (other.CompareTag("Destructible"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(attackPower);
        }
    }
}
