using UnityEngine;
using System.Collections;

public class RangedEnemy : AIBaseClass {

 

    public Rigidbody bulletPrefab;
    public Transform firePoint;
    public float fireRate;


	protected override void Awake ()
    {
        base.Awake();
        _actionAvailable = true;
	}

    public void ShootAtPlayer()
    {
        Vector3 direction = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);

        transform.LookAt(direction);

        if(_actionAvailable)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        _actionAvailable = false;

        yield return new WaitForSeconds(Random.Range(fireRate + .1f, fireRate + .75f));

        _actionAvailable = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            StartCoroutine(Stun());
    }
}
