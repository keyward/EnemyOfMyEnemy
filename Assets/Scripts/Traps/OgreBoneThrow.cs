using UnityEngine;
using System.Collections;

public class OgreBoneThrow : MonoBehaviour {


   
    public GameObject bonePrefab;
    public float lookSpeed;
    private Transform playerTransform;
    private Transform firePoint;
    private bool _throwBones;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        firePoint = transform.GetChild(0);
        _throwBones = false;
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
            if (_throwBones)
            {
                yield return new WaitForSeconds(Random.Range(.1f, .75f));

                Instantiate(bonePrefab, firePoint.position, firePoint.rotation);

                yield return new WaitForSeconds(2f);
            }
            else
                yield return null;
        }
    }

    void LookAtPlayer()
    {
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _throwBones = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _throwBones = false;
    }
}
