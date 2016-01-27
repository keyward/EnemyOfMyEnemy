using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {



    public GameObject deathParticles;
    private NavMeshAgent pathFinder;

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();

        StartCoroutine(CrawlAround());
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Bullet"))
        {
            Instantiate(deathParticles, transform.position, Quaternion.Euler(90f, 0f, 0f));
            Destroy(gameObject);
        }
    }

    IEnumerator CrawlAround()
    {
        while (true)
        {
            Vector3 nextPosition = new Vector3(Random.Range(transform.position.x - 10f, transform.position.x + 10f), //x
                                               0f,                                                                   //y
                                               Random.Range(transform.position.z - 10f, transform.position.z + 10f));//z

            pathFinder.SetDestination(nextPosition);

            while (pathFinder.remainingDistance > .05f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }
    }
}
