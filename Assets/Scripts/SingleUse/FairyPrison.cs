using UnityEngine;
using System.Collections;

public class FairyPrison : MonoBehaviour {

    public GameObject[] fairySpawner;
    private bool spawnerActivated;


    void Awake()
    {
        spawnerActivated = false;

        foreach (GameObject spawnFairy in fairySpawner)
            spawnFairy.SetActive(false);

        GetComponent<MeshRenderer>().enabled = false;
    }


	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Moe") && !spawnerActivated)
        {
            spawnerActivated = true;
            foreach (GameObject spawnFairy in fairySpawner)
                spawnFairy.SetActive(true);
        }
    }
}
