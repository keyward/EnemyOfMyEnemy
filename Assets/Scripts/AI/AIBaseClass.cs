using UnityEngine;
using System.Collections;

public class AIBaseClass : MonoBehaviour {
    
    [Header("AI Base")]

    // Attack
    protected float _lungeSmoothing;
    protected bool _actionAvailable;

    // Navigation
    protected Transform _playerTransform;
    protected NavMeshAgent _pathFinder;

    // Sounds
    protected AudioSource _enemyAudio;
    public AudioClip[] enemySounds;

    // Animation
    protected Animator _aiAnimator;
    private int _attackAnimation;

   
    protected virtual void Awake ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _pathFinder = GetComponent<NavMeshAgent>();

        _enemyAudio = GetComponent<AudioSource>();

        _aiAnimator = GetComponent<Animator>();
        _attackAnimation = Animator.StringToHash("Attack");


        _lungeSmoothing = 5f;
        _actionAvailable = true;
	}
	
    // -- Temporarily disable enemy -- //
    protected virtual IEnumerator Stun()
    {
        _actionAvailable = false;

        _enemyAudio.clip = enemySounds[1];
        _enemyAudio.Play();

        if(_pathFinder)
            _pathFinder.Stop();

        yield return new WaitForSeconds(2f);

        _actionAvailable = true;

        if(_pathFinder)
            _pathFinder.Resume();
    }

    // -- Melee attack -- //
    protected virtual IEnumerator Lunge()
    {
        if (!_actionAvailable)
            yield break;

        print(gameObject.name + "  start lunge");

        _aiAnimator.SetTrigger(_attackAnimation);
        _actionAvailable = false;

        _enemyAudio.clip = enemySounds[0];
        _enemyAudio.Play();

        Vector3 target = _playerTransform.position;
        _pathFinder.SetDestination(target);
        

        float initialSpeed = _pathFinder.speed;
        float initialAccel = _pathFinder.acceleration;

        _pathFinder.speed = 7f;
        _pathFinder.acceleration = 16f;

        float timeCheck = 0f;

        // dash towards player -- attack
        while (Vector3.Distance(transform.position, target) > .5f)
        {
            if (timeCheck >= 2f)
                break;

            timeCheck += Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _lungeSmoothing);
            yield return null;
        }

        _pathFinder.speed = initialSpeed;
        _pathFinder.acceleration = initialAccel;

        _aiAnimator.enabled = false;

        yield return new WaitForSeconds(1f);

        _actionAvailable = true;
        _aiAnimator.enabled = true;
        print(gameObject.name+"   end lunge");
    }
}