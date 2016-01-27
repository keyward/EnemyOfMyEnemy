using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

    public GameObject creditCanvas;
    public RectTransform scrollingText;
    public float moverUpper = .2f;

    void Start()
    {
        creditCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(CreditScroll());
        }
    }

  

    IEnumerator CreditScroll()
    {
        creditCanvas.SetActive(true);
       

        while(scrollingText.anchoredPosition.y <= 250f)
        {
            scrollingText.anchoredPosition = new Vector3(0f, scrollingText.anchoredPosition.y + moverUpper, 0f) ;

            yield return null;
        }

        Application.Quit();
    }
}
