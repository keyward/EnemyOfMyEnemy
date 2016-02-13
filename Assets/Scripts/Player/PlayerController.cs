using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    public MeshRenderer playerFront;
    public Rigidbody bulletPrefab;
    public Transform firePoint;
    public Color damageColor;
    public AudioSource damagedSound;
    public AudioSource dashSound;


    private Rigidbody _playerControls;
    private Moe _moeScript;
    private Renderer _render;
    private float moveSpeed;
    private float diveSpeed;
    private float _damageCoolDownSpeed;
    private bool _canRoll;
    private bool _canShoot;
    private bool _invincible;
    


    void Awake()
    {
        _moeScript = GameObject.FindGameObjectWithTag("Moe").GetComponent<Moe>();
        _playerControls = GetComponent<Rigidbody>();
        _render = GetComponent<Renderer>();
        _damageCoolDownSpeed = GetComponent<Health>().damageCoolDown;


        moveSpeed = 6f;
        diveSpeed = 1300f;

        _canRoll = true;
        _canShoot = true;

        _invincible = false;
    }
	
	void FixedUpdate ()
    {
        // -- left thumbstick -- //
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        // -- right thumbstick -- //
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
        if (Input.GetAxis("Fire2") > 0)
            StartCoroutine(DiveRoll(horz, vert));
    }

    void Update()
    {
        // player shooting
        if (Input.GetAxis("Fire1") > 0f)
            StartCoroutine(ShootPea());


        if (Input.GetButtonDown("Fire3"))
            Taunt();
    }

    void MovePlayer(float hAxis, float vAxis)
    {
        Vector3 movement = new Vector3(hAxis, 0f, vAxis);
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        _playerControls.MovePosition(transform.position + movement);      
    }

    void RotatePlayer(float hAxis, float vAxis)
    {
        float angle = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;

        if (angle == 0f)
            return;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    void Taunt()
    {
        StartCoroutine(_moeScript.BullCharge());
    }

    IEnumerator DiveRoll(float rHAxis, float rVAxis)
    {
        if (!_canRoll)
            yield break;

        _canRoll = false;

        
        Vector3 diveRoll = new Vector3(rHAxis, 0f, rVAxis);
        diveRoll = diveRoll.normalized * diveSpeed * Time.deltaTime;
        _playerControls.AddForce(diveRoll, ForceMode.Impulse);

        dashSound.pitch = Random.Range(.8f, 1.1f);
        dashSound.Play();

        _render.material.color = Color.magenta;

        yield return new WaitForSeconds(2f);

        _render.material.color = Color.red;
        _canRoll = true;
    }

    IEnumerator TakeDamage()
    {
        if (_invincible)
            yield break;


        _invincible = true;
        _render.material.color = damageColor;

        CameraController.Instance.ScreenShake(.1f);

        damagedSound.pitch = Random.Range(.8f, 1.6f);
        damagedSound.Play();

        yield return new WaitForSeconds(_damageCoolDownSpeed);

        _invincible = false;
        _render.material.color = Color.red;
    }

    IEnumerator ShootPea()
    {
        if (!_canShoot)
            yield break;

        playerFront.enabled = false;
        _canShoot = false;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        yield return new WaitForSeconds(1f);

        playerFront.enabled = true;
        _canShoot = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Damage"))
            StartCoroutine(TakeDamage());
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Damage"))
            StartCoroutine(TakeDamage());
    }
}
