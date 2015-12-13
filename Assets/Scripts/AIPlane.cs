using UnityEngine;
using System.Collections;

public class AIPlane : MonoBehaviour {

    public int FlightSpeed;

    private GameController controller;

	// Use this for initialization
	void Start () {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }
	
	void FixedUpdate () {
	    if (controller.Running)
        {
            transform.position += Vector3.left * FlightSpeed;
        }
	}
}
