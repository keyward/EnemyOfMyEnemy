using UnityEngine;
using System.Collections;

public class RangedTrigger : MonoBehaviour {



    public GameObject[] rangedEnemies;
    private AIBaseClass _rangedScript;
    private bool activated;


    void Awake()
    {
        activated = false;

        foreach(GameObject ranged in rangedEnemies)
        {
            _rangedScript = ranged.GetComponent<AIBaseClass>();
            _rangedScript.enabled = false;
        } 
    }

    // if player enters trigger, allow all the ranged enemies to shoot
	void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if(other.CompareTag("Player"))
        {
            activated = true;

            foreach (GameObject ranged in rangedEnemies)
            {
                _rangedScript = ranged.GetComponent<AIBaseClass>();
                _rangedScript.enabled = true;
            }
        }
    }
	
	
}
