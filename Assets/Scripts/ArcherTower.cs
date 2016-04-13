using UnityEngine;
using System.Collections;

public class ArcherTower : MonoBehaviour {

    public Rigidbody archerOnTower;

    void OnDestroy()
    {
        archerOnTower.isKinematic = false;
    }
}
