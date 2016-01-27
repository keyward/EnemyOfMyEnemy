using UnityEngine;
using System.Collections;

public class Wall_ChargeDestroy : MonoBehaviour {



    public GameObject FinalParticles;

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Moe"))
            return;

        col.gameObject.GetComponent<Moe>().StartCoroutine("FollowCheck");
        Instantiate(FinalParticles, transform.position, FinalParticles.transform.rotation);
        Destroy(gameObject);
    }
}
