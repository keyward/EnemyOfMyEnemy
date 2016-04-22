using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health {

    private Transform playerRespawnPoint;
    private PlayerController _playerRef;
    private Image _playerHealthImage;
    private int _initialHealth;

    void Awake ()
    {
        _initialHealth = health;

        _damageAudio = GetComponent<AudioSource>();
        _playerRef = GetComponent<PlayerController>();
        _playerHealthImage = GameObject.FindGameObjectWithTag("PosterCanvas").transform.FindChild("PlayerHealth").GetComponent<Image>();

        ChangeChainGraphic();
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);

        if (health <= 0)
        {
            RespawnPlayer();
            return;
        }
           
        ChangeChainGraphic();
        CameraController.Instance.ScreenShake(.1f);
    }

    public override void PlayerCheckPoint(Transform newSpawnPoint)
    {
        playerRespawnPoint = newSpawnPoint;
        health = _initialHealth;
        ChangeChainGraphic();
    }

    // -- Send player to alotted respawn point -- //
    void RespawnPlayer()
    {
        health = _initialHealth;

        _damageAudio.clip = damageSoundEffects[1];
        _damageAudio.Play();

        _playerRef.StopCoroutine("DashAnimEvent");

        if (playerRespawnPoint)
            transform.position = playerRespawnPoint.position;

        ChangeChainGraphic();

        if (_playerRef.posterInventory.Count > 0)
            _playerRef.posterInventory.Clear();
    }

    void ChangeChainGraphic()
    {
        switch (health)
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
    }
}
