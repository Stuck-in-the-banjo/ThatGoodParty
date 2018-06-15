using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esfera : MonoBehaviour {

    public float radius = 5.0f;
    public float offset = 0.0f;

    public float speed = 2.0f;

    public bool counter_clock = false;

    public GameObject orbiting_object;

    // Use this for initialization
    void Start () {

        if(counter_clock)
            transform.position = new Vector3((Mathf.Sin(radius / (offset * Mathf.Deg2Rad)) * radius) + orbiting_object.transform.position.x, (Mathf.Cos(-radius / (offset * Mathf.Deg2Rad)) * radius) + orbiting_object.transform.position.y);
        else transform.position = new Vector3((Mathf.Sin(radius / (offset * Mathf.Deg2Rad)) * radius) + orbiting_object.transform.position.x, (Mathf.Cos(radius / (offset * Mathf.Deg2Rad)) * radius) + orbiting_object.transform.position.y);

    }
	
	// Update is called once per frame
	void Update () {

        offset += speed * Time.deltaTime;

        if(counter_clock)
            transform.position = new Vector3((radius * Mathf.Sin(offset * Mathf.Deg2Rad)) + orbiting_object.transform.position.x, -(radius * Mathf.Cos(offset * Mathf.Deg2Rad)) + orbiting_object.transform.position.y);
        else transform.position = new Vector3((radius * Mathf.Sin(offset * Mathf.Deg2Rad)) + orbiting_object.transform.position.x, (radius * Mathf.Cos(offset * Mathf.Deg2Rad)) + orbiting_object.transform.position.y); 
            
    }

   
   
}
