using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour {

 
    [Header("Ranged Enemy")]

    // Ranged Attack
    private Transform _playerTransform;
    public Rigidbody bulletPrefab;
    public Transform firePoint;
    public float fireDelay;
    [Range(0, 4)] public float accuracyOffset;

    // States
    private bool _stunned;
    private bool _actionAvailable;
    [HideInInspector] public bool looking;
   
    // Particles
    public ParticleSystem _stunParticles;

    // Animations
    private Animator _aiAnimator;
    private int _shootAnimation;

    // Audio
    private AudioSource _enemyAudio;
    public AudioClip[] enemySounds;


	void Awake ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _aiAnimator = GetComponent<Animator>();
        _enemyAudio = GetComponent<AudioSource>();

        _actionAvailable = true;
        _stunned = false;
        looking = false;

        _shootAnimation = Animator.StringToHash("Shooting");
        _stunParticles.Stop();
	}

    void Update()
    {
        if(looking)
        {
            transform.LookAt(_playerTransform);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        }
    }

    public void LookAtPlayer()
    {
        if (_stunned)
            return;

        if (!looking)
            looking = true;


        // Make the archer shoot with some inaccuracy
        Vector3 offSetDirection = new Vector3(_playerTransform.position.x + Random.Range(-accuracyOffset, accuracyOffset), 
                                              _playerTransform.position.y, 
                                              _playerTransform.position.z + Random.Range(-accuracyOffset, accuracyOffset));

        firePoint.transform.LookAt(offSetDirection);

       
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
        _stunned = true;
        _stunParticles.Play();
        yield return new WaitForSeconds(5f);
        _stunned = false;
        _stunParticles.Stop();
    }
}
