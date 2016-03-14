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

            // give player back their health - change the spawn point
            Health playerHealth = other.GetComponent<Health>();
            playerHealth.playerRespawnPoint = spawnPoint;
            playerHealth.health = 5;

            torchParticles.SetActive(true);
        }
    }
}
