using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEnder : MonoBehaviour
{
    public int newLevel;

    IEnumerator EndLevel()
    {

        float fadeTime = GameObject.Find("PRE_Fader").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(newLevel);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        StartCoroutine(EndLevel());
    }
}
