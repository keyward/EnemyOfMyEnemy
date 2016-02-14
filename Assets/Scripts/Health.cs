using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {


    public int health;
    public float damageCoolDown;
    public GameObject deathParticles;
    public Color hurtColor;
    [HideInInspector] public Transform playerRespawnPoint;

    private int _initialHealth;
    private bool _invincible;


    void Awake()
    {
        _initialHealth = health;
        _invincible = false;
    }

    // -- Remove health from object -- kill if necessary -- //
    public void TakeDamage(int damageAmount)
    {
        if (_invincible)
            return;

        health -= damageAmount;

        StartCoroutine(DamageCooldown());

        gameObject.GetComponent<Renderer>().material.color = hurtColor;

        if (health > 0)
            return;

        if (gameObject.CompareTag("Player"))
            RespawnPlayer();
        else
            Die();
    }

    IEnumerator DamageCooldown()
    {
        _invincible = true;

        yield return new WaitForSeconds(damageCoolDown);

        _invincible = false;
    }

    void Die()
    {
        CameraController.Instance.ScreenShake(.1f);

        Instantiate(deathParticles, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }

    // -- Send player to alotted respawn point -- //
    void RespawnPlayer()
    {
        if(playerRespawnPoint)
            transform.position = playerRespawnPoint.position;

        health = _initialHealth;
    }
}
