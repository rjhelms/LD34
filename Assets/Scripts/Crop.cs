using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{

    public Sprite deadSprite;
    public Sprite liveSprite;

    public bool isLive = false;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = deadSprite;
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
        }
    }
}
