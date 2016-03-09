using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {


    [SerializeField] private LevelTrigger _levelTriggerRef;

	void Awake ()
    {
        _levelTriggerRef = transform.parent.GetComponent<LevelTrigger>();
	}
	
	public void RemoveEnemy()
    {
        _levelTriggerRef.MinusOneEnemy();
    }
}
