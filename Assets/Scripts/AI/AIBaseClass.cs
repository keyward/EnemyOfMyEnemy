using UnityEngine;
using System.Collections;

public class AIBaseClass : MonoBehaviour {


    #region Variables
    public Color stunColor;
    public int damageAmount;


    protected Transform _playerTransform;
    protected NavMeshAgent _pathFinder;
    protected Renderer _objectRender;
    protected Color _initialColor;
    protected float _smoothDamp;
    protected bool _actionAvailable;
    #endregion

    protected virtual void Awake ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _pathFinder = GetComponent<NavMeshAgent>();
        _objectRender = GetComponent<Renderer>();

        _smoothDamp = 10f;// used for lunge speed //

        _actionAvailable = true;
	}
	
    // -- Temporarily disable enemy -- //
    protected virtual IEnumerator Stun()
    {
        _actionAvailable = false;

        _initialColor = _objectRender.material.color;
        _objectRender.material.color = stunColor;

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

        Vector3 target = _playerTransform.position;

        _actionAvailable = false;

        // dash towards player -- attack
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _smoothDamp);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        _actionAvailable = true;
    }
}