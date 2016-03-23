using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    // Shooting
    public Rigidbody bulletPrefab;
    public Transform firePoint;

    // Audio
    public AudioClip[] playerSoundEffects;
    private AudioSource _playerSounds;
    // 0 Dash 
    // 1 Shoot
    // 2 Taunt

    // Components
    private Rigidbody _playerControls;
    private MoeAI _moeScript;
    private Renderer _render;

    // Animations
    private Animator _playerAnimator;
    private int _dashAnim;
    private int _shootAnim;

    // Attributes
    private float moveSpeed;
    private float moveSpeedModifier;
    private float diveSpeed;
    private float horz;
    private float vert;

    // Abilities
    [HideInInspector] public bool tauntDisabled;
    private bool canTaunt;
    private bool _canRoll;
    private bool _canShoot;


    void Awake()
    {
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<MoeAI>();
        _playerControls = GetComponent<Rigidbody>();
        _playerSounds = GetComponent<AudioSource>();

        _playerAnimator = GetComponent<Animator>();
        _dashAnim = Animator.StringToHash("Dash");
        _shootAnim = Animator.StringToHash("Shoot");

        // Player metrics
        moveSpeed = 6f;
        moveSpeedModifier = 1f;
        diveSpeed = 1300f;

        _canRoll = true;
        _canShoot = true;
        canTaunt = true;
        tauntDisabled = false;
    }

    void FixedUpdate()
    {
        // -- left thumbstick controls -- //
        horz = Input.GetAxisRaw("LeftHorz");
        vert = Input.GetAxisRaw("LeftVert");

        // -- right thumbstick controls -- //
        float rightHorz = Input.GetAxis("RightHorz");
        float rightVert = Input.GetAxis("RightVert");


        // move player with left stick
        if (horz != 0f || vert != 0f)
            MovePlayer(horz, vert);

        // if player doesn't aim with right stick, aim with left
        if (rightHorz == 0 && rightVert == 0)
            RotatePlayer(horz, vert);
        else
            RotatePlayer(rightHorz, rightVert);

        // dive roll
        if (Input.GetAxis("Dash") > 0)
            StartCoroutine(DiveRoll(horz, vert));
    }

    void Update()
    {
        // player shooting
        if (Input.GetAxis("Shoot") > 0f)
            StartCoroutine(ShootPea());


        if (Input.GetButtonDown("Taunt") && canTaunt && !tauntDisabled)
            StartCoroutine(Taunt());

        //Restart Level
        if (Input.GetKeyDown(KeyCode.Q))
            Application.LoadLevel(0);

        //Exit Game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void MovePlayer(float hAxis, float vAxis)
    {
        Vector3 movement = new Vector3(hAxis, 0f, vAxis);
        movement = movement.normalized * (moveSpeed * moveSpeedModifier) * Time.deltaTime;

        _playerControls.MovePosition(transform.position + movement);
    }

    void RotatePlayer(float hAxis, float vAxis)
    {
        float angle = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;

        if (angle == 0f)
            return;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    IEnumerator Taunt()
    {
        canTaunt = false;

        _playerSounds.clip = playerSoundEffects[2];
        _playerSounds.Play();

        _moeScript.ChangeState(MoeAI.aiState.charging);

        yield return new WaitForSeconds(1f);

        canTaunt = true;
    }

    IEnumerator DiveRoll(float rHAxis, float rVAxis)
    {
        if (!_canRoll)
            yield break;

        _canRoll = false;

        _playerAnimator.SetTrigger(_dashAnim);
        yield return new WaitForSeconds(2f);

        _canRoll = true;
    }

    void DashAnimEvent()
    {
        _playerSounds.clip = playerSoundEffects[0];
        _playerSounds.Play();

        Vector3 diveRoll = new Vector3(horz, 0f, vert);
        diveRoll = diveRoll.normalized * diveSpeed;

        _playerControls.AddForce(diveRoll * Time.deltaTime, ForceMode.Impulse);
    }

    IEnumerator ShootPea()
    {
        if (!_canShoot)
            yield break;

        _canShoot = false;

        _playerAnimator.SetTrigger(_shootAnim);

        yield return new WaitForSeconds(1f);

        _canShoot = true;
    }

    void ShootAnimEvent()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        _playerSounds.clip = playerSoundEffects[1];
        _playerSounds.Play();
    }

	public float MoveSpeedModifier {
		get {
			return moveSpeedModifier;
		}
		set {
			moveSpeedModifier = value;
		}
	}
}
