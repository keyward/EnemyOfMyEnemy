using UnityEngine;
using System.Collections;

public class RangedEnemyShot : MonoBehaviour {

    private RangedEnemy _rangedEnemy;


	void Awake ()
    {
        _rangedEnemy = transform.parent.GetComponent<RangedEnemy>();
	}
	
	
	void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            _rangedEnemy.ShootAtPlayer();
    }
}
