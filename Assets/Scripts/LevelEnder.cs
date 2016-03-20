using UnityEngine;
using System.Collections;

public class LevelEnder : MonoBehaviour
{
    public int newLevel;

    IEnumerator EndLevel()
    {

        float fadeTime = GameObject.Find("PRE_Fader").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(5);
        Application.LoadLevel(newLevel);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        StartCoroutine(EndLevel());
    }
}
