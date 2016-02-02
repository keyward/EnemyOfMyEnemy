using UnityEngine;
using System.Collections;

public class OgreBoneThrow : MonoBehaviour {



    public Transform firePoint;
    public GameObject bonePrefab;
    public GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void Start ()
    {
        StartCoroutine(ThrowBone());
	}
	
    void Update()
    {
        transform.LookAt(player.transform);

        firePoint.transform.LookAt(player.transform);
    }

    IEnumerator ThrowBone()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(.1f, .75f));

            Instantiate(bonePrefab, firePoint.position, firePoint.rotation);

            yield return new WaitForSeconds(2f);
        }
    }
}
