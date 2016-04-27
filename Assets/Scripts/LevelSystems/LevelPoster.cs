 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPoster : MonoBehaviour {


    public Sprite posterToDisplay;
    public GameObject _yParticle;
    public AudioSource _soundClip;
    public bool _getsDestroyed;
    public GameObject destroyParticles;
    public int posterIndex;

    private PlayerController _playerAbilities;
    private GameObject _posterPanel;
    private Image _canvasImage;
    private bool _posterTurnedOn;
    private bool _inputEnabled;
    

    void Awake()
    {
        _posterPanel = GameObject.FindGameObjectWithTag("PosterCanvas").transform.FindChild("PosterPanel").gameObject;
        _playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (!_posterPanel)
            print("TURN THE CANVAS BACK ON");
        else
            _canvasImage = _posterPanel.transform.FindChild("PosterImage").GetComponent<Image>();

        _posterTurnedOn = false;
        _inputEnabled = false;
    }

    void Start()
    {
        if (_posterPanel.activeInHierarchy)
             _posterPanel.SetActive(false);
    }

    void Update()
    {
        if(_inputEnabled && Input.GetButtonDown("Interact"))
        {
            ShowPoster(); 
        }
        else if(_inputEnabled && Input.GetButton("Cancel") && _posterTurnedOn == true)
        {
            ShowPoster();
        }
    }

    void ShowPoster()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        _posterTurnedOn = !_posterTurnedOn;
        _soundClip.Play();
        _posterPanel.SetActive(_posterTurnedOn);

        PlayerPrefs.SetInt(posterIndex.ToString(), 1);

        //destroys poster and spawns particle
        if (_getsDestroyed == true && _posterTurnedOn == false)
        {
            Instantiate(destroyParticles, transform.position, destroyParticles.transform.rotation);
            Destroy(gameObject);
        }
    }

    // when player walks up to the poster - slot the correct image, and allow them to activate the canvas
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerAbilities.tauntDisabled = true;
            _canvasImage.sprite = posterToDisplay;
            _yParticle.SetActive(true);
            _inputEnabled = true;
        }  
    }
    
    // when player leaves - disable the canvas and input
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Time.timeScale != 1)
                Time.timeScale = 1f;

            _playerAbilities.tauntDisabled = false;
            _inputEnabled = false;
            _posterPanel.SetActive(false);
            _yParticle.SetActive(false);
        }
    }
}
