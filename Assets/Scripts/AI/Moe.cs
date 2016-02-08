using UnityEngine;
using System.Collections;

public class Moe : MonoBehaviour {




    #region Components
    public GameObject areaDamage;
    public GameObject chargeDamage;
    public GameObject slamParticles;
    public Transform targetEnemy;

    [Header("Audio")]
    public AudioSource stompAttack;
    public AudioSource chargeAttack;
    private Transform playerPosition;
    [HideInInspector] public NavMeshAgent pathFinder;
    private Renderer _moeRender;
    private Color initialColor;
    private Vector3 startPos;
    #endregion

    #region Attack()
    public float attackCooldown;
    private float raiseHeight;
    private float raiseMagnitude;
    private float fallSpeed;
    #endregion

    #region States
    public bool _actionAvailable;
    
    
    public bool _isFollowing;
    private bool _canCharge;
    private bool _scared;
    private bool _attacking;
    #endregion


    void Start ()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        pathFinder = GetComponent<NavMeshAgent>();

        _moeRender = GetComponent<Renderer>();
        initialColor = _moeRender.material.color;

        areaDamage.SetActive(false);
        chargeDamage.SetActive(false);

        raiseHeight = 2f;
        raiseMagnitude = .1f;
        fallSpeed = 1f;

        _actionAvailable = true;
        _canCharge = true;
        _isFollowing = false;
        _scared = false;
        _attacking = false;

        StartCoroutine("FollowCheck");
	}

	void Update ()
    {
        if (_isFollowing)
            FollowTarget();
	}

    void FollowTarget()
    {
        if (pathFinder.isOnNavMesh)
        {
            if (targetEnemy != null)
                pathFinder.SetDestination(targetEnemy.position);

            else if (Vector3.Distance(transform.position, playerPosition.position) > 5f)
                pathFinder.SetDestination(playerPosition.position);
        }
    }

    public IEnumerator Attack()
    {
        if (!_actionAvailable || _scared)
            yield break;


        // -- disabling Moe navigation/abilities -- //
        _actionAvailable = false;
        _isFollowing = false;
        _attacking = true;
        startPos = transform.position;
        pathFinder.enabled = false;
        // ------------------------------------------------------ //

        print("scared: " + _scared);
        // -- raising Moe up -- //
        for (float i = 0; i < raiseHeight; i += .1f)
        {
            if(_scared)           
                break;
            
            transform.position = new Vector3(transform.position.x, transform.position.y + raiseMagnitude, transform.position.z);
            yield return null;
        }

        // -- pause in the air for effect -- //
        yield return new WaitForSeconds(1f);

        // -- whether or not moe is scared, he will come down -- //
        while(transform.position != startPos)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, fallSpeed);
            yield return null;
        }

        print("scared: " + _scared);
        // -- if Moe isn't Scared after coming down, activate damage and visualFX -- //
        if (!_scared)
        {
            // Effects
            Instantiate(slamParticles, transform.position, Quaternion.Euler(90f, 0f, 0f));
            CameraController.Instance.ScreenShake(.2f);
            stompAttack.Play();

            // actual attack
            areaDamage.SetActive(true);
            yield return new WaitForSeconds(.1f);
            areaDamage.SetActive(false);
        }
        print("scared: " + _scared);

        // -- enable Moe navigation/abilities -- //
        pathFinder.enabled = true;
        _attacking = false;
        yield return new WaitForSeconds(attackCooldown);
        _actionAvailable = true;
    }

    public IEnumerator BullCharge()
    {
        if (!_actionAvailable || _scared || !_canCharge)
            yield break;

        StartCoroutine("BreakTimer");

        _moeRender.material.color = Color.red;

        // deactivate AI behavior
        _actionAvailable = false;
        _canCharge = false;
        _isFollowing = false;

        //get normal values
        float normalSpeed = pathFinder.speed;
        float normalAcceleration = pathFinder.acceleration;
        float normalAngularSpeed = pathFinder.angularSpeed;
        float normalStoppingDistance = pathFinder.stoppingDistance;

        //getting charge position
        Vector3 target = playerPosition.position;
        pathFinder.Stop();
        pathFinder.SetDestination(target);

        yield return new WaitForSeconds(.2f);

        //set charge values
        pathFinder.speed = 50f;
        pathFinder.acceleration = 100f;
        pathFinder.angularSpeed = 360f;
        pathFinder.stoppingDistance = 0f;
        pathFinder.Resume();

        chargeDamage.SetActive(true);


        chargeAttack.Play();
        while (pathFinder != null && pathFinder.remainingDistance > .01f)
        {
            // if moe gets scared by a spider, he will halt his charge //
            if (_scared)
            {
                pathFinder.Stop();
                pathFinder.SetDestination(this.transform.position);

                yield return new WaitForSeconds(.25f);
                break;
            }

            // no spiders around = full charge 
            else
                yield return null;     
        }

        //charge over, reset normal values
        chargeDamage.SetActive(false);
        pathFinder.ResetPath();
        pathFinder.speed = normalSpeed;
        pathFinder.acceleration = normalAcceleration;
        pathFinder.angularSpeed = normalAngularSpeed;
        pathFinder.stoppingDistance = normalStoppingDistance;

        //cool down for abilities after charging
        yield return new WaitForSeconds(.75f);

        _actionAvailable = true;
        StopCoroutine("BreakTimer");

        //cool down for charging itself
        yield return new WaitForSeconds(4f);
        _canCharge = true;
        _moeRender.material.color = initialColor;
    }

    IEnumerator FollowCheck()
    {
        while(true)
        {
            if (_actionAvailable)
                _isFollowing = true;

            yield return new WaitForSeconds(1.5f);
        }  
    }    

    IEnumerator Shake()
    {
        _scared = true;
        _actionAvailable = false;
        _isFollowing = false;

        StopCoroutine("Attack");
        if(_attacking)
        {
            transform.position = startPos;
            _attacking = false;
        }
           
 

        float shakeLength = 10f;
        float shakeDampen = .3f;

        while(shakeLength >= 0f)
        {
            transform.localPosition += Random.insideUnitSphere * shakeDampen;
            shakeLength -= .15f;

            yield return null;
        }

        _scared = false;
        _actionAvailable = true;
    }

    IEnumerable BreakTimer()
    {
        float breakTime = 3f;

        while(breakTime >= 0f)
        {
            breakTime -= Time.deltaTime;
            yield return null;
        }

        if(transform.position != startPos)
            transform.position = startPos;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Destructible"))
        {
            targetEnemy = other.gameObject.transform;

            if(_actionAvailable)
                StartCoroutine("Attack");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fear"))
        {
            _scared = true;
            StopCoroutine("Shake");
            StartCoroutine("Shake");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fear"))
            _scared = false;

        if (other.gameObject.CompareTag("Shield"))
            _isFollowing = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Shield"))
        {
            _actionAvailable = false;
            _isFollowing = false;
            print("hit shield");
        }  
    }
}