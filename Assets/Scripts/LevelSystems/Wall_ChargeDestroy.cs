using UnityEngine;
using System.Collections;

public class Wall_ChargeDestroy : MonoBehaviour {



    public GameObject destroyParticles;
    public bool canBeDestroyed;

    void Awake()
    {
        canBeDestroyed = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage") && canBeDestroyed)
        {
            Instantiate(destroyParticles, transform.position, destroyParticles.transform.rotation);
            Destroy(gameObject);
        }
    }
}
