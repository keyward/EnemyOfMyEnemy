using UnityEngine;
using System.Collections;

public class ParticleStop : MonoBehaviour {


    public GameObject sandParticles;

    void Start()
    {
        if (sandParticles.activeInHierarchy)
            sandParticles.SetActive(false);
    }

    void OnBecameInvisible()
    {
        sandParticles.SetActive(false);
        print("invisible");
    }

    void OnBecameVisible()
    {
        sandParticles.SetActive(true);
    }
	
	
}
