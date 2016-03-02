 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPoster : MonoBehaviour {


    // get canvas by tag
    // Insert Img to display
    public Sprite posterToDisplay;

    private GameObject _posterCanvas;
    private Image _canvasImage;
    private bool _posterTurnedOn;
    private bool _inputEnabled;
    

    void Awake()
    {
        _posterCanvas = GameObject.FindGameObjectWithTag("PosterCanvas");
        _canvasImage = _posterCanvas.transform.FindChild("PosterImage").GetComponent<Image>();
        print(_posterCanvas);

        _posterTurnedOn = false;
        _inputEnabled = false;
    }

    void Start()
    {
        if (_posterCanvas.activeInHierarchy)
            _posterCanvas.SetActive(false);
    }

    void Update()
    {
        if(_inputEnabled && Input.GetButtonDown("Fire2"))
        {
            _posterTurnedOn = !_posterTurnedOn;
            _posterCanvas.SetActive(_posterTurnedOn); 
        }
    }

    // when player walks up to the poster - slot the correct image, and allow them to activate the canvas
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canvasImage.sprite = posterToDisplay;
            _inputEnabled = true;
        }  
    }
    
    // when player leaves - disable the canvas and input
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _inputEnabled = false;
            _posterCanvas.SetActive(false);
        }
    }
}
