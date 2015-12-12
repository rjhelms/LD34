using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float RotationSpeed = 5;
    public float FlightSpeed = 1;

    public Sprite[] planeSprites;

    private float rotation = 0;
    private Transform sprayArea;

    // Use this for initialization
    void Start()
    {
        sprayArea = GameObject.Find("SprayArea").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // calculate rotation
        rotation -= Input.GetAxis("Vertical") * RotationSpeed;

        if (rotation < 0)
        {
            rotation += 360;
        }
        else if (rotation > 360)
        {
            rotation -= 360;
        }

        // update sprite based on rotation
        int spriteIndex = Mathf.FloorToInt((rotation + 22) / 45);
        if (spriteIndex >= planeSprites.Length)
        {
            spriteIndex = 0;
        }
        this.GetComponent<SpriteRenderer>().sprite = planeSprites[spriteIndex];

        // rotate spray area
        Quaternion target = Quaternion.Euler(0, 0, rotation);
        sprayArea.rotation = target;

        // disable spray area if not flying level

        if (22f < rotation && rotation < 338f)
        {
            sprayArea.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            sprayArea.GetComponent<Collider2D>().enabled = true;
        }

        // fly plane
        float xSpeed = Mathf.RoundToInt(Mathf.Cos(rotation * Mathf.Deg2Rad) * FlightSpeed);
        float ySpeed = Mathf.RoundToInt(Mathf.Sin(rotation * Mathf.Deg2Rad) * FlightSpeed);

        transform.position += new Vector3(xSpeed, ySpeed);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Crash! " + collision.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger! " + collision.gameObject);
        if (collision.gameObject.tag == "Terrain")
        {
            Debug.Log("Into terrain!");
            Debug.Log("You lose!");
        }
    }
}
