using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEnder : MonoBehaviour
{
    public int newLevel;

    IEnumerator EndLevel()
    {
        GameObject.Find("PRE_Fader").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(newLevel);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerRef = other.GetComponent<PlayerController>();
            for(int i = 0; i<playerRef.posterInventory.Count; i++)
            {
                PlayerPrefs.SetInt(i.ToString(), 1);
            }

            StartCoroutine(EndLevel());
        }   
    }
}
