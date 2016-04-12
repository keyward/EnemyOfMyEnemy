﻿using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {


    public float moveSpeed;
    public GameObject deathParticles;

    private EnemyManager _enemyManageRef;
    private Vector3 _spawnLocation;

    void Awake()
    {
        _spawnLocation = transform.position;

        StartCoroutine(CrawlAround());
    }

    void OnEnable()
    {
        if(gameObject.name != "StaticPixie(Clone)")
            _enemyManageRef = GameObject.FindGameObjectWithTag("EnemyMgr").GetComponent<EnemyManager>();
    }


    // -- pixie is destroyed -- //
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Bullet"))
        {
            Instantiate(deathParticles, transform.position, Quaternion.Euler(90f, 0f, 0f));

            if (gameObject.name != "StaticPixie(Clone)")
                _enemyManageRef.RemoveEnemy();

            Destroy(gameObject);
        }
    }

    IEnumerator CrawlAround()
    {
        while (true)
        {
            // get a random position while staying within bounds //
            Vector3 nextPosition = new Vector3(Random.Range(_spawnLocation.x - 10f, _spawnLocation.x + 10f),   //x
                                               _spawnLocation.y,                                                             //y
                                               Random.Range(_spawnLocation.z - 10f, _spawnLocation.z + 10f));  //z

            float stickTimer = 0;

            // lerp to position //
            while(Vector3.Distance(transform.position, nextPosition) > 2f)
            {
                if (stickTimer > 1f)
                    break;

                stickTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * moveSpeed);
                yield return null;
            }

            // brief pause before moving again -- toggled of for the time being //
            //yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }
    }
}
