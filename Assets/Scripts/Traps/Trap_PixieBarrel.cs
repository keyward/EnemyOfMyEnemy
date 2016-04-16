using UnityEngine;
using System.Collections;

public class Trap_PixieBarrel : MonoBehaviour {

    public GameObject pixieSpawner;

    void Awake()
    {
        pixieSpawner.SetActive(false);
        pixieSpawner.transform.parent = null;
    }

	void OnDestroy()
    {
        pixieSpawner.SetActive(true);
    }
}
