using UnityEngine;
using System.Collections;

public class DefenderGear : MonoBehaviour {


    public GameObject shield;
    private DefenderEnemy defender;
    private Moe _moeScript;


    void Awake()
    {
        defender = transform.parent.gameObject.GetComponent<DefenderEnemy>();
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<Moe>();
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
            DestroyShield();
    }

    // -- Destroy the defender shield and change his behaviour to attack the player -- //
    void DestroyShield()
    {
        // able to be killed by Moe now //
        defender.gameObject.tag = "Enemy";

        // moe can move now as well //
        _moeScript._actionAvailable = true;
        _moeScript._isFollowing = true;

        // defender is vulnerable //
        defender._shieldActive = false;
        defender._healthScript.enabled = true;
        Destroy(gameObject);
        Destroy(shield);
    }
}
