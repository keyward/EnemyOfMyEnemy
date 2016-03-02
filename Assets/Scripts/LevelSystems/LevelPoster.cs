 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPoster : MonoBehaviour {


    public Sprite posterToDisplay;

    private GameObject _posterCanvas;
    private Image _canvasImage;
    private bool _posterTurnedOn;
    private bool _inputEnabled;
    

    void Awake()
    {
        _posterCanvas = GameObject.FindGameObjectWithTag("PosterCanvas");
        _canvasImage = _posterCanvas.transform.FindChild("PosterImage").GetComponent<Image>();

        if(_posterCanvas.activeInHierarchy)
            _posterCanvas.SetActive(false);

        _posterTurnedOn = false;
        _inputEnabled = false;
    }

    void Update()
    {
        if(_inputEnabled && Input.GetButtonDown("Fire2"))
        {
            _posterTurnedOn = !_posterTurnedOn;
            _posterCanvas.SetActive(_posterTurnedOn);
            _canvasImage.sprite = posterToDisplay;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _inputEnabled = true;
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _inputEnabled = false;
            _posterCanvas.SetActive(false);
            _posterTurnedOn = false;
        } 
    }
}
