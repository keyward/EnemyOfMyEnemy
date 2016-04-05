using UnityEngine;
using System.Collections;

public class OgreBoneThrow : MonoBehaviour {


   
    public GameObject bonePrefab;
    public float lookSpeed;
    private Transform playerTransform;
    private Transform firePoint;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        firePoint = transform.GetChild(0);
    }

	void Start ()
    {
        StartCoroutine(ThrowBone());
	}
	
    void Update()
    {
        LookAtPlayer();

        firePoint.transform.LookAt(playerTransform);
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

    void LookAtPlayer()
    {
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }
}
