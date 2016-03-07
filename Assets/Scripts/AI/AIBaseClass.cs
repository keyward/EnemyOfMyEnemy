using UnityEngine;
using System.Collections;

public class AIBaseClass : MonoBehaviour {
    
    [Header("AI Base")]
    // Attack
    public int attackPower;
    protected float _lungeSmoothing;
    protected bool _actionAvailable;

    // Colors 
    public Color stunColor;
    protected Renderer _objectRender;
    protected Color _initialColor;

    // Navigation
    protected Transform _playerTransform;
    protected NavMeshAgent _pathFinder;

    // Sounds
    protected AudioSource _enemyAudio;
    public AudioClip[] enemySounds;

   
    protected virtual void Awake ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _pathFinder = GetComponent<NavMeshAgent>();
        _objectRender = GetComponent<Renderer>();

        _enemyAudio = GetComponent<AudioSource>();

        _lungeSmoothing = 10f;
        _actionAvailable = true;
	}
	
    // -- Temporarily disable enemy -- //
    protected virtual IEnumerator Stun()
    {
        _actionAvailable = false;

        _initialColor = _objectRender.material.color;
        _objectRender.material.color = stunColor;

        _enemyAudio.clip = enemySounds[1];
        _enemyAudio.Play();

        if(_pathFinder)
            _pathFinder.Stop();

        yield return new WaitForSeconds(2f);

        _actionAvailable = true;
        _objectRender.material.color = _initialColor;

        if(_pathFinder)
            _pathFinder.Resume();
    }

    // -- Melee attack -- //
    protected virtual IEnumerator Lunge()
    {
        if (!_actionAvailable)
            yield break;

        _actionAvailable = false;

        Vector3 target = _playerTransform.position;
        _enemyAudio.clip = enemySounds[0];
        _enemyAudio.Play();
        
        // dash towards player -- attack
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _lungeSmoothing);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _actionAvailable = true;
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