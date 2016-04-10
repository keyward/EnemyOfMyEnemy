using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {


    // Health Stats
    [SerializeField] protected int health;
    public float damageCoolDown;
    protected bool _invincible;

    // Death
    public GameObject deathParticles;

    // Audio
    public AudioClip[] damageSoundEffects;
    protected AudioSource _damageAudio;
    // 0 damage sound
    // 1 death  sound

 
    void Awake()
    {
        _invincible = false;

        _damageAudio = GetComponent<AudioSource>();
    }

    // -- Remove health from object -- kill if necessary -- //
    public virtual void TakeDamage(int damageAmount)
    {
        if (_invincible)
            return;

        _damageAudio.clip = damageSoundEffects[0];
        _damageAudio.Play();

        health -= damageAmount;

        StartCoroutine(DamageCooldown());

        // -- Death -- //
        if (health <= 0 && !gameObject.CompareTag("Player"))
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

    public virtual void PlayerCheckPoint(Transform newSpawnPoint) { }
}
