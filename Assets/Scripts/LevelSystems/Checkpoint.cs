using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {


    public Transform spawnPoint;
    public GameObject torchParticles;

    private bool _activatedCheckpoint;

	void Awake ()
    {
        _activatedCheckpoint = false;
        torchParticles.SetActive(false);
	}
	
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_activatedCheckpoint)
        {
            _activatedCheckpoint = true;
            other.GetComponent<Health>().playerRespawnPoint = spawnPoint;
            torchParticles.SetActive(true);
        }
    }
}
