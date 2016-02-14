using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {


    public Transform spawnPoint;


	void Start ()
    {
	
	}
	
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Health>().playerRespawnPoint = spawnPoint;
    }
}
