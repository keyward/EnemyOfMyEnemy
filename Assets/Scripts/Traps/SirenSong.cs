using UnityEngine;
using System.Collections;

public class SirenSong : MonoBehaviour {


    //pull player towards siren 
    /*
        player enters trigger zone
            pull player towards siren song

    */

    public float pullSpeed;
    private Vector3 pullDifference;
    private GameObject _player;
    private bool _pulling;

	
	void Start ()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pulling = false;
	} 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(PullPlayerIn());
    }


    IEnumerator PullPlayerIn()
    {
        if (_pulling)
            yield break;

        _pulling = true;

        for(float i = 3; i > 0; i -= Time.deltaTime)
        {
            
            pullDifference = transform.position - _player.transform.position;
            _player.transform.position += pullDifference.normalized * Time.deltaTime * pullSpeed;
            yield return null;
        }

        _pulling = false;
    }
}
