using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public GUITexture RenderTexture;

    private Rect screenRect;
    private float pixelRatioAdjustment = 1.6f;  // aspect ratio of a CGA screen

    // Use this for initialization
    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        RenderTexture.pixelInset = screenRect;
        float aspectRatio = screenRect.width / screenRect.height;
        RenderTexture.transform.localScale = new Vector3(1, aspectRatio * pixelRatioAdjustment);
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != screenRect.width || Screen.height != screenRect.height)
        {
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
            RenderTexture.pixelInset = screenRect;
            float aspectRatio = screenRect.width / screenRect.height;
            RenderTexture.transform.localScale = new Vector3(1, aspectRatio * pixelRatioAdjustment);
        }
    }
}
