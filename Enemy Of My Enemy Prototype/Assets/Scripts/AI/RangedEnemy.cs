using UnityEngine;
using System.Collections;

public class RangedEnemy : AIBaseClass {

 

    public Rigidbody bulletPrefab;
    public Transform firePoint;


	protected override void Awake ()
    {
        base.Awake();
	}

    void Start()
    {
        StartCoroutine(RangedAttack());
    }

    void Update()
    {
        Vector3 direction = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);

        transform.LookAt(direction);

        firePoint.transform.LookAt(_playerTransform);
    }

    public IEnumerator RangedAttack()
    {
        while(true)
        {
            if (_canAttack)
            {
                yield return new WaitForSeconds(Random.Range(.1f, .75f));

                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                yield return new WaitForSeconds(2f);
            }
            else
                yield return null;
        }
        
    }
}
