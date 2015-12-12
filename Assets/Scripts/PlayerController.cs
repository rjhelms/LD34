using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float RotationSpeed = 5;
    public float FlightSpeed = 1;

    public Sprite[] planeSprites;

    private float rotation = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotation -= Input.GetAxis("Vertical") * RotationSpeed;

        if (rotation < 0)
        {
            rotation += 360;
        }
        else if (rotation > 360)
        {
            rotation -= 360;
        }


        float xSpeed = Mathf.RoundToInt(Mathf.Cos(rotation * Mathf.Deg2Rad) * FlightSpeed);
        float ySpeed = Mathf.RoundToInt(Mathf.Sin(rotation * Mathf.Deg2Rad) * FlightSpeed);

        transform.position += new Vector3(xSpeed, ySpeed);

        int spriteIndex = Mathf.FloorToInt((rotation + 22) / 45);
        if (spriteIndex >= planeSprites.Length)
        {
            spriteIndex = 0;
        }
        this.GetComponent<SpriteRenderer>().sprite = planeSprites[spriteIndex];
    }
}
