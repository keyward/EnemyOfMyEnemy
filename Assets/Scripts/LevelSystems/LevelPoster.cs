 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPoster : MonoBehaviour {


    // get canvas by tag
    // Insert Img to display
    public Image posterToDisplay;

    private GameObject _posterCanvas;
    private Image _canvasImage;
    private bool _posterTurnedOn;
    

    void Awake()
    {
        _posterCanvas = GameObject.FindGameObjectWithTag("PosterCanvas");
        _canvasImage = _posterCanvas.transform.GetChild(2).GetComponent<Image>();

        _posterCanvas.SetActive(false);
        _posterTurnedOn = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Fire3"))
        {
            _posterTurnedOn = !_posterTurnedOn;
            _posterCanvas.SetActive(_posterTurnedOn);
            _canvasImage = posterToDisplay;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        _posterCanvas.SetActive(false);
    }
}
