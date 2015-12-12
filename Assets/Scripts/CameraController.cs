using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform Player;
    public float CameraXOffset = 16;
    public float CameraYOffset = 16;
    public float CameraZOffset = -10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Player)
        {
            transform.position = new Vector3(Player.position.x + CameraXOffset, Player.position.y + CameraYOffset, CameraZOffset);
        }
	}
}
