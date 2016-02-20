using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    [Header("Player Spawn Point")] public Transform spawnPoint;

    [Header("Torch Stuff")]
    public GameObject torchParticles;

    private bool _activatedCheckpoint;
    private MeshRenderer _boxVisibility;

	void Awake ()
    {
        _activatedCheckpoint = false;
        torchParticles.SetActive(false);
        _boxVisibility = GetComponent<MeshRenderer>();
	}
	
    
    void OnTriggerEnter(Collider other)
    {
        if (_activatedCheckpoint == true)
            return;

        if (other.CompareTag("Player"))
        {
            _activatedCheckpoint = true;
            other.GetComponent<Health>().playerRespawnPoint = spawnPoint;
            torchParticles.SetActive(true);
        }
    }
}
