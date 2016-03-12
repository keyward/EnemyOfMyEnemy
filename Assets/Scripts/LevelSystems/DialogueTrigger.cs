using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]

public class DialogueTrigger : MonoBehaviour {


    public AudioSource _dialogueClip;
    private bool _triggerActivated;

    void Start()
    {
        _triggerActivated = false;
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_triggerActivated)
            StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        _triggerActivated = true;

        _dialogueClip.Play();

        while (_dialogueClip.isPlaying)
            yield return null;

        Destroy(gameObject);
    }
}
