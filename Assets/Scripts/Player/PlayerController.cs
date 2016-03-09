using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    // Shooting
    public Rigidbody bulletPrefab;
    public Transform firePoint;
    
    // Sounds
    public AudioClip[] playerSoundEffects;
    private AudioSource _playerSounds;

    // Components
    private Rigidbody _playerControls;
    private MoeAI _moeScript;
    private Renderer _render;
  
    // Attributes
    private float moveSpeed;
	private float moveSpeedModifier;
    private float diveSpeed;

    // Abilities
    [HideInInspector] public bool canTaunt;
    private bool _canRoll;
    private bool _canShoot;


    void Awake()
    {
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<MoeAI>();
        _playerControls = GetComponent<Rigidbody>();
        _playerSounds = GetComponent<AudioSource>();

        // Player metrics
        moveSpeed = 6f;
		moveSpeedModifier = 1f;
        diveSpeed = 1300f;

        _canRoll = true;
        _canShoot = true;
        canTaunt = true;
    }
	
	void FixedUpdate ()
    {
        // -- left thumbstick controls -- //
        float horz = Input.GetAxisRaw("LeftHorz");
        float vert = Input.GetAxisRaw("LeftVert");

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


        if (Input.GetButtonDown("Taunt") && canTaunt)
            StartCoroutine(Taunt());
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

        _moeScript.currentState = MoeAI.aiState.charging;

        yield return new WaitForSeconds(1f);

        canTaunt = true;
    }

    IEnumerator DiveRoll(float rHAxis, float rVAxis)
    {
        if (!_canRoll)
            yield break;

        _canRoll = false;

        _playerSounds.clip = playerSoundEffects[0];
        _playerSounds.Play();
        
        Vector3 diveRoll = new Vector3(rHAxis, 0f, rVAxis);
        diveRoll = diveRoll.normalized * diveSpeed * Time.deltaTime;
        _playerControls.AddForce(diveRoll, ForceMode.Impulse);

        yield return new WaitForSeconds(2f);

        _canRoll = true;
    }

    IEnumerator ShootPea()
    {
        if (!_canShoot)
            yield break;

        _canShoot = false;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // play shoot sound
        _playerSounds.clip = playerSoundEffects[1];
        _playerSounds.Play();

        yield return new WaitForSeconds(1f);

        _canShoot = true;
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
