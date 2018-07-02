using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCollidersTrip4 : MonoBehaviour {

    public GameObject cloudColliders;
    public GameObject starRoad;
    public GameObject player;

    void Start()
    {
        cloudColliders.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        starRoad.SetActive(false);
        player.GetComponent<Player>().floor = 110.0f;
        player.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }
}
