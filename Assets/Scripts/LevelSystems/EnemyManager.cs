using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {


    private LevelTrigger _levelTriggerRef;

	void Awake ()
    {
        if(_levelTriggerRef)
            _levelTriggerRef = transform.parent.GetComponent<LevelTrigger>();
	}
	
	public void RemoveEnemy()
    {
        if(_levelTriggerRef)
            _levelTriggerRef.MinusOneEnemy();
    }
}
