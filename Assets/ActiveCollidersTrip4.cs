using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCollidersTrip4 : MonoBehaviour {

    public GameObject cloudColliders;
    public GameObject player;

    void Start()
    {
        cloudColliders.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        cloudColliders.SetActive(true);
        player.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }
}
