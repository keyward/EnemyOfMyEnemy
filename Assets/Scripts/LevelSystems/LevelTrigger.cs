using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour {

    [Header("Drag Spawn Points Here")]
    public GameObject[] enemySpawners;
    private bool triggerActivated;

    
    void Awake()
    {
        triggerActivated = false;

        foreach(GameObject spawner in enemySpawners)
            spawner.SetActive(false);
    }


    void OnTriggerEnter(Collider other)
    {
        if(triggerActivated)
            return;

        if (other.CompareTag("Player"))
        {
            triggerActivated = true;

            foreach (GameObject spawner in enemySpawners)
                spawner.SetActive(true);
        }  
    }
}
