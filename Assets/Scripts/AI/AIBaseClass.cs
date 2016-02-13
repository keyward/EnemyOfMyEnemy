using UnityEngine;
using System.Collections;

public class AIBaseClass : MonoBehaviour {


    #region Variables
    public Color stunColor;

    protected Transform _playerTransform;
    protected NavMeshAgent _pathFinder;
    protected Renderer _objectRender;
    protected Color _initialColor;
    public bool _canAttack;
    #endregion

    protected virtual void Awake ()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _pathFinder = GetComponent<NavMeshAgent>();
        _objectRender = GetComponent<Renderer>();

        _canAttack = true;
	}
	
    protected virtual IEnumerator Stun()
    {
        _canAttack = false;

        _initialColor = _objectRender.material.color;
        _objectRender.material.color = stunColor;

        if(_pathFinder)
            _pathFinder.Stop();

        yield return new WaitForSeconds(2f);

        _canAttack = true;
        _objectRender.material.color = _initialColor;

        if(_pathFinder)
            _pathFinder.Resume();
    }
}