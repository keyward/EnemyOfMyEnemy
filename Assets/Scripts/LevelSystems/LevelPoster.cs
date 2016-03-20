 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPoster : MonoBehaviour {


    public Sprite posterToDisplay;
    public GameObject _yParticle;
    public AudioSource _soundClip;

    private GameObject _posterPanel;
    private Image _canvasImage;
    private bool _posterTurnedOn;
    private bool _inputEnabled;
    

    void Awake()
    {
        _posterPanel = GameObject.FindGameObjectWithTag("PosterCanvas").transform.FindChild("PosterPanel").gameObject;

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
            _posterTurnedOn = !_posterTurnedOn;
            _soundClip.Play();
            _posterPanel.SetActive(_posterTurnedOn); 
        }
    }

    // when player walks up to the poster - slot the correct image, and allow them to activate the canvas
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            _inputEnabled = false;
            _posterPanel.SetActive(false);
            _yParticle.SetActive(false);
        }
    }
}
