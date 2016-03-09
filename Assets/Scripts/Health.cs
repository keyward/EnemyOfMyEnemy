using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {


    // Health Stats
    public int health;
    public float damageCoolDown;
    private int _initialHealth;
    private bool _invincible;

    public GameObject deathParticles;
    [HideInInspector] public Transform playerRespawnPoint;

    // Sounds
    public AudioClip[] damageSoundEffects;
    /*
        0 - damage sound
        1 - death sound --- for player: Respawn Sound
    */
    private AudioSource _damageAudio;


    void Awake()
    {
        _initialHealth = health;
        _invincible = false;

        _damageAudio = GetComponent<AudioSource>(); 
    }

    // -- Remove health from object -- kill if necessary -- //
    public void TakeDamage(int damageAmount)
    {
        if (_invincible)
            return;

        if (gameObject.CompareTag("Player"))
        {
            // room for more specific player functionality here
            CameraController.Instance.ScreenShake(.1f);
        }

        // play sound
        _damageAudio.clip = damageSoundEffects[0];
        _damageAudio.Play();

        // subtract object health
        health -= damageAmount;

        // make object invincible briefly
        StartCoroutine(DamageCooldown());


        // -- Death -- //
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
        //_objectColor.material.color = hurtColor;

        yield return new WaitForSeconds(damageCoolDown);

        _invincible = false;
        //_objectColor.material.color = _initialColor;
    }

    // Destroy object
    void Die()
    {
        _damageAudio.clip = damageSoundEffects[1];
        _damageAudio.Play();

        CameraController.Instance.ScreenShake(.1f);

        Instantiate(deathParticles, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }

    // -- Send player to alotted respawn point -- //
    void RespawnPlayer()
    {
        _damageAudio.clip = damageSoundEffects[1];
        _damageAudio.Play();

        if (playerRespawnPoint)
            transform.position = playerRespawnPoint.position;

        health = _initialHealth;
    }
}
