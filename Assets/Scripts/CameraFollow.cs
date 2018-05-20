using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float start_follow_distance = 10.0f;

    public float offset;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(player.transform.position.y >= (transform.position.y + offset))
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y - offset, transform.position.z);
            return;
        }

        if (player.transform.position.y <= (transform.position.y - offset))
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y + offset, transform.position.z);
            return;
        }      
	}
}
