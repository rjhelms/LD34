using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float rotationSpeed = 5;
    public float rotation = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotation -= Input.GetAxis("Vertical") * rotationSpeed;

        if (rotation < 0)
        {
            rotation += 360;
        }
        else if (rotation > 360)
        {
            rotation -= 360;
        }
        Quaternion target = Quaternion.Euler(0, 0, rotation);
        transform.rotation = target;
    }
}
