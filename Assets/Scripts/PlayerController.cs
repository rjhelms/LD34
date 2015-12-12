using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float RotationSpeed = 5;
    public float FlightSpeed = 3;

    public float StallSpeed = 1;
    public float OutofStallSpeed = 2.5f;
    public float CriticalAngle = 45;
    public float AdjustSpeed = 0.1f;
    public float StallRotation = 1f;
    public float CurrentSpeed;

    public float LevelFlightLerpT = 0.25f;

    public bool IsStalled = false;

    public Sprite[] planeSprites;
    public Sprite[] crashSprites;

    public float CameraXOffset = 16;
    public float CameraYOffset = 16;
    public float CameraZOffset = -10;

    private float rotation = 0;
    private Transform sprayArea;

    private GameController controller;

    private bool isTakeoff;
    private Transform WorldCamera;

    #region MonoBehaviours

    // Use this for initialization
    void Start()
    {
        sprayArea = GameObject.Find("SprayArea").transform;
        CurrentSpeed = 0;
        isTakeoff = true;
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        WorldCamera = GameObject.Find("WorldCamera").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controller.Running)
        {
            ProcessInput();
            FlightDynamics();
            UpdateSprite();
            HandleSprayer();

            // fly plane
            float xSpeed = Mathf.RoundToInt(Mathf.Cos(rotation * Mathf.Deg2Rad) * CurrentSpeed);
            float ySpeed = Mathf.RoundToInt(Mathf.Sin(rotation * Mathf.Deg2Rad) * CurrentSpeed);

            transform.position += new Vector3(xSpeed, ySpeed);
            WorldCamera.position = new Vector3(transform.position.x + CameraXOffset, transform.position.y + CameraYOffset, CameraZOffset);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            Debug.Log("You lose!");
            if (rotation < 90 || rotation >= 270)
            {
                this.GetComponent<SpriteRenderer>().sprite = crashSprites[0];
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = crashSprites[1];
            }
            controller.Running = false;
        }

        if (collision.gameObject.tag == "LevelEnd")
        {
            if (rotation < 22f || rotation > 338f)
            {
                Debug.Log("Landed!");
                Debug.Log("You win!");
                ScoreManager.Instance.Score += 50;
                controller.Running = false;
            }
        }
    }

    #endregion MonoBehaviours

    #region Private methods

    private void FlightDynamics()
    {
        if (isTakeoff)
        {
            if (CurrentSpeed >= FlightSpeed * 0.9f)
            {
                isTakeoff = false;
            }
            else
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, FlightSpeed, LevelFlightLerpT);
            }
        }
        else
        {
            if (rotation > CriticalAngle && rotation < (180 - CriticalAngle))
            {
                // if above critical angle up, reduce speed
                CurrentSpeed -= AdjustSpeed;
            }

            else if (rotation > (180 + CriticalAngle) && rotation < (360 - CriticalAngle))
            {
                // if below critical angle down, increase speed
                CurrentSpeed += AdjustSpeed;
                //Debug.Log("Flying down " + currentSpeed);
            }

            else
            {
                // if flying level, lerp back to FlightSpeed
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, FlightSpeed, LevelFlightLerpT);
            }

            if (CurrentSpeed <= StallSpeed && !IsStalled)
            {
                IsStalled = true;
            }


            if (IsStalled)
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
                if (CurrentSpeed >= OutofStallSpeed)
                {
                    IsStalled = false;
                }
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
