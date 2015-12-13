using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{

    public Sprite DeadSprite;
    public Sprite LiveSprite;

    public bool IsLive = false;

    private GameController controller;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = DeadSprite;
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sprayer")
        {
            if (!IsLive)
            {
                IsLive = true;
                this.GetComponent<SpriteRenderer>().sprite = LiveSprite;
                controller.PlayCropNoise();
            }
        }
    }
}
