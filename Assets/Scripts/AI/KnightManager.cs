using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnightManager : MonoBehaviour {

	private List<MeleeEnemy> enemyList;

	void Start()
	{
		this.enemyList = new List<MeleeEnemy> ();
	}

	public void addEnemyToList(MeleeEnemy enemy)
	{
		print ("adding enemy" + enemy.ToString());
		this.enemyList.Add (enemy);
	}

	public void removeEnemyFromList(MeleeEnemy enemy)
	{
		this.enemyList.Remove (enemy);
	}

	public bool checkForAttackers(MeleeEnemy enemy)
	{
		List<MeleeEnemy> testList = new List<MeleeEnemy> (this.enemyList);
		testList.Remove(enemy);

		foreach (MeleeEnemy e in testList)
		{
			if (e.attacking == true)
			{
				return true;
			}
		}
		return false;
	}
}
