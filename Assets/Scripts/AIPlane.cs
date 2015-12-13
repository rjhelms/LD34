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
            if (transform.position.x < -16)
                transform.position = new Vector3(controller.LevelX + 16, transform.position.y, transform.position.z);

        }
	}
}
