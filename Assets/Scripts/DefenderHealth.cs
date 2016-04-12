using UnityEngine;
using System.Collections;

public class DefenderHealth : Health {

    private DefenderEnemy _defenderScript;

	void Start ()
    {
        _defenderScript = GetComponent<DefenderEnemy>();
	}

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);

        _defenderScript.defenderAnimator.SetTrigger(_defenderScript.shieldBreakTrigger);
    }
}
