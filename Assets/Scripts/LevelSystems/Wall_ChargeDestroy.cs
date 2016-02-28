using UnityEngine;
using System.Collections;

public class Wall_ChargeDestroy : MonoBehaviour {



    public GameObject FinalParticles;
    public bool canBeDestroyed;

    void Awake()
    {
        canBeDestroyed = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage") && canBeDestroyed)
        {
            Instantiate(FinalParticles, transform.position, FinalParticles.transform.rotation);
            Destroy(gameObject);
        }
    }
}
