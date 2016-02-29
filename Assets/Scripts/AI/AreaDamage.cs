using UnityEngine;
using System.Collections;

public class AreaDamage : MonoBehaviour {


    public float explosionForce;
    public float explosionRadius;
    public int attackPower;


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            // if Moe hits the player -- only do 1 damage
            if (other.CompareTag("Player"))
                other.gameObject.GetComponent<Health>().TakeDamage(1);
            
            // if its anything else, deal selected amount
            else
                other.gameObject.GetComponent<Health>().TakeDamage(attackPower);

            // push away whatever got hit
            if(other.gameObject.GetComponent<Rigidbody>())
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
    }
}
