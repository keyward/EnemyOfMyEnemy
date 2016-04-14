using UnityEngine;
using System.Collections;

public class DefenderGear : MonoBehaviour {


    public GameObject shield;
    private DefenderEnemy defender;


    void Awake()
    {
        defender = transform.parent.gameObject.GetComponent<DefenderEnemy>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            DestroyShield();
    }

    void DestroyShield()
    {
        defender.ShieldBreak();       

        Destroy(gameObject);

        //instantiate particles at the shields location
        Destroy(shield);
    }
}
