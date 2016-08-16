using UnityEngine;
using System.Collections;

public class CanvasAssist : MonoBehaviour {


    public GameObject gameCanvas;


	void Awake ()
    {
        gameCanvas.SetActive(true);
	}
}
