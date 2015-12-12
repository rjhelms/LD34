using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float RotationSpeed = 5;
    public float FlightSpeed = 3;

    public float StallSpeed = 1;
    public float OutofStallSpeed = 2.5f;
    public float CriticalAngle = 45;
    public float AdjustSpeed = 0.1f;
    public float StallRotation = 1f;

    public float LevelFlightLerpT = 0.25f;

    public Sprite[] planeSprites;

    private float currentSpeed;
    private float rotation = 0;
    private Transform sprayArea;
    private bool isStalled;

    #region MonoBehaviours

    // Use this for initialization
    void Start()
    {
        sprayArea = GameObject.Find("SprayArea").transform;
        currentSpeed = FlightSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        ProcessInput();
        FlightDynamics();
        UpdateSprite();
        HandleSprayer();

        // fly plane
        float xSpeed = Mathf.RoundToInt(Mathf.Cos(rotation * Mathf.Deg2Rad) * currentSpeed);
        float ySpeed = Mathf.RoundToInt(Mathf.Sin(rotation * Mathf.Deg2Rad) * currentSpeed);

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

    #endregion MonoBehaviours

    #region Private methods

    private void FlightDynamics()
    {
        if (rotation > CriticalAngle && rotation < (180 - CriticalAngle))
        {
            // if above critical angle up, reduce speed
            currentSpeed -= AdjustSpeed;
            Debug.Log("Flying up " + currentSpeed);
        }

        else if (rotation > (180 + CriticalAngle) && rotation < (360 - CriticalAngle))
        {
            // if below critical angle down, increase speed
            currentSpeed += AdjustSpeed;
            Debug.Log("Flying down " + currentSpeed);
        }

        else
        {
            // if flying level, lerp back to FlightSpeed
            currentSpeed = Mathf.Lerp(currentSpeed, FlightSpeed, LevelFlightLerpT);
            Debug.Log("Level " + currentSpeed);
        }

        if (currentSpeed <= StallSpeed && !isStalled)
        {
            isStalled = true;
            Debug.Log("Stalled!");
        }

        if (isStalled)
        {
            if (rotation < 90 || rotation > 270)
            {
                rotation -= StallRotation;
                if (rotation < 0) rotation += 360;
            }
            else if (rotation > 90 || rotation < 270)
            {
                rotation += StallRotation;
            }
            if (currentSpeed >= OutofStallSpeed)
            {
                isStalled = false;
                Debug.Log("Out of stall!");
            }
        }
    }

    private void HandleSprayer()
    {
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
    }

    private void ProcessInput()
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
    }

    private void UpdateSprite()
    {
        int spriteIndex = Mathf.FloorToInt((rotation + 22) / 45);
        if (spriteIndex >= planeSprites.Length)
        {
            spriteIndex = 0;
        }
        this.GetComponent<SpriteRenderer>().sprite = planeSprites[spriteIndex];
    }

    #endregion Private Methods
}
