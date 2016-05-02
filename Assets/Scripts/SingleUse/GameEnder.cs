using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEnder : MonoBehaviour {

    public Fading fader;

	void Start()
    {
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(6f);

        fader.BeginFade(1);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(0);
    }
}
