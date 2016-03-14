using UnityEngine;
using System.Collections;

public class NarwhalHornDamage : MonoBehaviour {

    [Header("Damage Done To Player")]
    public int damageAmount;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Health>().TakeDamage(damageAmount);
            print("player Hit");
        }
           
    }
}
