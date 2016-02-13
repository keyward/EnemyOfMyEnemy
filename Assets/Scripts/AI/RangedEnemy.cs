﻿using UnityEngine;
using System.Collections;

public class RangedEnemy : AIBaseClass {

 

    public Rigidbody bulletPrefab;
    public Transform firePoint;
    public float fireRate;

   


	protected override void Awake ()
    {
        base.Awake();
        _canAttack = true;
	}

    public void ShootAtPlayer()
    {
        Vector3 direction = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);

        transform.LookAt(direction);

        if(_canAttack)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        _canAttack = false;

        yield return new WaitForSeconds(Random.Range(fireRate + .1f, fireRate + .75f));

        _canAttack = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            StartCoroutine(Stun());
    }
}
