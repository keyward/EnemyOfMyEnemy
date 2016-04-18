using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEnder : MonoBehaviour {

	void Start()
    {
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene(0);
    }
}
