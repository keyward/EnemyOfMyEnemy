using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


    //Singleton
    public static CameraController Instance;

    //Follow Player
    public Vector3 cameraOffset = new Vector3(0, 11, -11);
    public float smoothing;
    private Transform player;

    //Screen Shake
    public float shakeIntensity;
    public float decreaseFactor;



    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = player.position + cameraOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }

    public void ScreenShake(float shakeLength)
    {
        StartCoroutine(StartScreenShake(shakeLength));
    }

    IEnumerator StartScreenShake(float shakeLgth)
    {
        Transform camTransform = gameObject.transform;
        Vector3 initialPosition = gameObject.transform.localPosition;


        while(shakeLgth >= 0)
        {
            camTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeIntensity;
            shakeLgth -= Time.deltaTime * decreaseFactor;

            yield return null;
        }

        camTransform.localPosition = initialPosition;
    }
}
