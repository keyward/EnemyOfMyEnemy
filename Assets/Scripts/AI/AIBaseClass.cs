﻿using UnityEngine;
using System.Collections;

public class AIBaseClass : MonoBehaviour {
    
    [Header("AI Base")]

    // Attack
    protected float _lungeSmoothing;
    protected bool _actionAvailable;
	protected bool _attackReady = false;

    // Navigation
    protected Transform _playerTransform;
    protected NavMeshAgent _pathFinder;

    // Sounds
    protected AudioSource _enemyAudio;
    public AudioClip[] enemySounds;

    // Animation
    protected Animator _aiAnimator;
    private int _attackAnimation;

    public ParticleSystem stunParticles;

   
    protected virtual void Awake ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _pathFinder = GetComponent<NavMeshAgent>();

        _enemyAudio = GetComponent<AudioSource>();

        _aiAnimator = GetComponent<Animator>();
        _attackAnimation = Animator.StringToHash("Attack");


        _lungeSmoothing = 5f;
        _actionAvailable = true;

        stunParticles.Stop();
	}
	
    // -- Temporarily disable enemy -- //
    protected virtual IEnumerator Stun()
    {
        _actionAvailable = false;
        stunParticles.Play();

        _enemyAudio.clip = enemySounds[1];
        _enemyAudio.Play();

        if(_pathFinder)
            _pathFinder.Stop();

        yield return new WaitForSeconds(5f);

        _actionAvailable = true;

        if(_pathFinder)
            _pathFinder.Resume();

        stunParticles.Stop();
    }

    // -- Melee attack -- //
    protected virtual IEnumerator Lunge()
    {
        if (!_actionAvailable)
            yield break;

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
    }

	protected virtual void Seek()
	{

	}

	protected virtual void MaintainDistance()
	{

	}

	protected virtual void Retreat()
	{

	}

	protected virtual void Attack()
	{

	}

}