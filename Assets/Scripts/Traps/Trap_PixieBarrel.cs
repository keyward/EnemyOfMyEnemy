using UnityEngine;
using System.Collections;

public class Trap_PixieBarrel : MonoBehaviour {

    public GameObject pixieSpawner;

    void Awake()
    {
        pixieSpawner.SetActive(false);
    }

	void OnDestroy()
    {
        pixieSpawner.SetActive(true);
    }
}
