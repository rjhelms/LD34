using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{

    public Sprite deadSprite;
    public Sprite liveSprite;

    public bool isLive = false;

    private GameController controller;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = deadSprite;
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
            isLive = true;
            this.GetComponent<SpriteRenderer>().sprite = liveSprite;
            ScoreManager.Instance.Score += 100;
            controller.PlayCropNoise();
        }
    }
}
