using UnityEngine;
using System.Collections;

public class MeleeEnemy : AIBaseClass {


    public float smoothDamp;
    private bool _navigating;


    protected override void Awake ()
    {
        base.Awake();

        _navigating = true;
	}
	
	void Update ()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) <= 2)
            StartCoroutine(MeleeAttack());

        if (_navigating && _canAttack)
        {
            if (_pathFinder.isActiveAndEnabled)
                _pathFinder.SetDestination(_playerTransform.position);
        }
    }

    private IEnumerator MeleeAttack()
    {
        if (!_canAttack)
            yield break;

        Vector3 target = _playerTransform.position;

        _navigating = false;
        _canAttack = false;
   
        // dash towards player -- attack
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothDamp);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _navigating = true;
        _canAttack = true;
    }
}
