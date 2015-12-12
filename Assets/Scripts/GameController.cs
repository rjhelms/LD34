using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public GUITexture RenderTexture;
    public Camera WorldCamera;

    public int TargetX = 160;
    public int TargetY = 200;

    private Rect screenRect;

    private float pixelRatioAdjustment;

    // Use this for initialization
    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        RenderTexture.pixelInset = screenRect;
        float aspectRatio = screenRect.width / screenRect.height;

        pixelRatioAdjustment = (float)TargetX / (float)TargetY;

        float xScale = 1;
        float yScale = 1;

        if (TargetX > TargetY)
        {
            yScale = aspectRatio * pixelRatioAdjustment;
            WorldCamera.orthographicSize = TargetX;
        }
        else
        {
            xScale = aspectRatio * pixelRatioAdjustment;
            WorldCamera.orthographicSize = TargetY;
        }


        RenderTexture.transform.localScale = new Vector3(xScale, yScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != screenRect.width || Screen.height != screenRect.height)
        {
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
            RenderTexture.pixelInset = screenRect;
            float aspectRatio = screenRect.width / screenRect.height;

            pixelRatioAdjustment = (float)TargetX / (float)TargetY;

            float xScale = 1;
            float yScale = 1;

            if (TargetX > TargetY)
            {
                yScale = aspectRatio * pixelRatioAdjustment;
                WorldCamera.orthographicSize = TargetX;
            }
            else
            {
                xScale = aspectRatio * pixelRatioAdjustment;
                WorldCamera.orthographicSize = TargetY;
            }


            RenderTexture.transform.localScale = new Vector3(xScale, yScale);
        }
    }
}
