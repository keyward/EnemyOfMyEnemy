using UnityEngine;
using System.Collections;

public class RangedEnemy : AIBaseClass {

 
    [Header("Ranged Enemy")]

    public Rigidbody bulletPrefab;
    public Transform firePoint;
    public float fireDelay;
    [Range(0, 4)] public float accuracyOffset;


    private bool _stunned;

    
    private int _shootAnimation;


	protected override void Awake ()
    {
        base.Awake();
        _actionAvailable = true;
        _stunned = false;

        _shootAnimation = Animator.StringToHash("Shooting");
	}

    public void LookAtPlayer()
    {
        if (_stunned)
            return;

        print("looking");
        // increase / decrease accuracy offset based on distance to the player

        // Make the archer shoot with some inaccuracy
        Vector3 direction = new Vector3(_playerTransform.position.x + Random.Range(-accuracyOffset, accuracyOffset), transform.position.y, _playerTransform.position.z + Random.Range(-accuracyOffset, accuracyOffset));
        firePoint.transform.LookAt(direction);

        // Look at the player when they are in range
        transform.LookAt(_playerTransform);

       
        // shoot at player if archer is able to 
        if (_actionAvailable)
        {
            _aiAnimator.SetTrigger(_shootAnimation);

            StartCoroutine(Reload());
        }
    }

    // Used in Animation Event 
    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        _enemyAudio.clip = enemySounds[0];
        _enemyAudio.Play();
    }

    IEnumerator Reload()
    {
        _actionAvailable = false;
        yield return new WaitForSeconds(Random.Range(fireDelay + .1f, fireDelay + .75f));
        _actionAvailable = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            StartCoroutine(Stunned());
    }

    IEnumerator Stunned()
    {
        print("stunned");
        _stunned = true;
        yield return new WaitForSeconds(5f);
        _stunned = false;
    }
}
