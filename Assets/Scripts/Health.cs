using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {


    // Health Stats
    public int health;
    public float damageCoolDown;
    private int _initialHealth;
    private bool _invincible;

    // Death
    public GameObject deathParticles;
    [HideInInspector] public Transform playerRespawnPoint;

    // Sounds
    // 0 - damage sound
    // 1 - death sound
    public AudioClip[] damageSoundEffects;
    private AudioSource _damageAudio;

    [SerializeField] private Image _playerHealthImage;


    void Awake()
    {
        _initialHealth = health;
        _invincible = false;

        _damageAudio = GetComponent<AudioSource>();

        if (gameObject.CompareTag("Player"))
        {
            _playerHealthImage = GameObject.FindGameObjectWithTag("PosterCanvas").transform.FindChild("PlayerHealth").GetComponent<Image>();
            ChangeChainGraphic();
        }
            
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && gameObject.CompareTag("Player"))
        {
            TakeDamage(5);
        }

    }

    // -- Remove health from object -- kill if necessary -- //
    public void TakeDamage(int damageAmount)
    {
        if (_invincible)
            return;

        if (gameObject.CompareTag("Player"))
        {
            ChangeChainGraphic();
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

    // *** ill come up with a better solution for this, im really tired right now *** //
    void ChangeChainGraphic()
    {
        // (5 - minus health) * .2f;
        switch(health)
        {
            case 5:
                _playerHealthImage.color = new Color(_playerHealthImage.color.r, _playerHealthImage.color.g, _playerHealthImage.color.b, 0.0f);
                break;
            case 4:
                _playerHealthImage.color = new Color(_playerHealthImage.color.r, _playerHealthImage.color.g, _playerHealthImage.color.b, 0.2f);
                break;
            case 3:
                _playerHealthImage.color = new Color(_playerHealthImage.color.r, _playerHealthImage.color.g, _playerHealthImage.color.b, 0.4f);
                break;
            case 2:
                _playerHealthImage.color = new Color(_playerHealthImage.color.r, _playerHealthImage.color.g, _playerHealthImage.color.b, 0.6f);
                break;
            case 1:
                _playerHealthImage.color = new Color(_playerHealthImage.color.r, _playerHealthImage.color.g, _playerHealthImage.color.b, 0.8f);
                break;
            case 0:
                _playerHealthImage.color = new Color(_playerHealthImage.color.r, _playerHealthImage.color.g, _playerHealthImage.color.b, 1.0f);
                break;
            default: 
                break;
        }
        print(health);
    }
}
