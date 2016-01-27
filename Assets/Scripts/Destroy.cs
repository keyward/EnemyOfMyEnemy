using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

    public AudioSource deathSound;

	
	void Start ()
    {
        if(deathSound)
        {
            deathSound = GetComponent<AudioSource>();

            deathSound.pitch = Random.Range(.9f, 1.3f);
            deathSound.Play();
        }
        

        Destroy(gameObject, 2f);
	}
}
