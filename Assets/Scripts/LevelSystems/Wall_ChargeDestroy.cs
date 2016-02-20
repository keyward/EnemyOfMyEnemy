using UnityEngine;
using System.Collections;

public class Wall_ChargeDestroy : MonoBehaviour {



    public GameObject FinalParticles;
    public bool canBeDestroyed;

    void Awake()
    {
        canBeDestroyed = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Moe") && canBeDestroyed)
        {
            col.gameObject.GetComponent<Moe>().StartCoroutine("FollowCheck");
            Instantiate(FinalParticles, transform.position, FinalParticles.transform.rotation);
            Destroy(gameObject);
        }
    }
}
