using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

    public AudioSource deathSound;
    public float timeUntilDestroyed;

	
	void Start ()
    {
        if(deathSound)
        {
            deathSound = GetComponent<AudioSource>();

            deathSound.pitch = Random.Range(.9f, 1.3f);
            deathSound.Play();
        }
        

        Destroy(gameObject, timeUntilDestroyed);
	}
}
