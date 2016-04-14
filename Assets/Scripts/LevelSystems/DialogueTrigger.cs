using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[RequireComponent(typeof (AudioSource))]

public class DialogueTrigger : MonoBehaviour {


    public AudioMixer audioManager;
    public AudioSource _dialogueClip;
    public GameObject _particle;
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
        _particle.SetActive(true);

        _dialogueClip.Play();
        audioManager.SetFloat("GameVolume", -15f);

        while (_dialogueClip.isPlaying)
            yield return null;

        audioManager.SetFloat("GameVolume", 0f);
        _particle.SetActive(false);
        Destroy(gameObject);
    }
}
