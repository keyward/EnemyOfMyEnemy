using UnityEngine;
using System.Collections;

public class PlayerPrefsTest : MonoBehaviour {

    int prefTest = 0;

    void Start()
    {
        print(PlayerPrefs.GetInt("PosterCount"));

        PlayerPrefs.SetInt("PosterCount", 5);

        print(PlayerPrefs.GetInt("PosterCount"));

        prefTest = PlayerPrefs.GetInt("PosterCount");

        print(prefTest);

        PlayerPrefs.SetInt("PosterCount", 10);

        print(PlayerPrefs.GetInt("PosterCount"));

        prefTest = PlayerPrefs.GetInt("PosterCount");

        print(prefTest);

    }
}
