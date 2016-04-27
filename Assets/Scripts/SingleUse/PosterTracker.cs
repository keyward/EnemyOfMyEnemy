using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PosterTracker : MonoBehaviour {

    // Player prefs (index, 0 = off 1 = on)

    public GameObject[] poster;
    public static PosterTracker Instance = null;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        UpdatePosters();
    }

    public void UpdatePosters()
    {
        for (int i = 0; i < poster.Length; i++)
        {
            if (PlayerPrefs.GetInt(i.ToString()) > 0)
                poster[i].SetActive(true);
            else
                poster[i].SetActive(false);
        }
    }
}
