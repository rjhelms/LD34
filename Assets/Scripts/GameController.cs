using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

    public Material RenderTexture;
    public Camera WorldCamera;
    public bool Running;

    public Text SpeedText;
    public Text StallText;
    public Text ScoreText;

    public int TargetX = 160;
    public int TargetY = 200;

    public float StallBlinkTime;

    private PlayerController player;
    private float pixelRatioAdjustment;
    private float speedFudgeFactor = 33.3f;

    private float nextStallBlink = -1;

    // Use this for initialization
    void Start()
    {
        pixelRatioAdjustment = (float)TargetX / (float)TargetY;
        Debug.Log(pixelRatioAdjustment);
        if (pixelRatioAdjustment <= 1)
        {
            RenderTexture.mainTextureScale = new Vector2(pixelRatioAdjustment, 1);
            RenderTexture.mainTextureOffset = new Vector2((1 - pixelRatioAdjustment) / 2, 0);
            WorldCamera.orthographicSize = TargetY / 2;
        } else
        {
            pixelRatioAdjustment = 1f / pixelRatioAdjustment;
            RenderTexture.mainTextureScale = new Vector2(1, pixelRatioAdjustment);
            RenderTexture.mainTextureOffset = new Vector2(0, (1 - pixelRatioAdjustment) / 2);
            WorldCamera.orthographicSize = TargetX / 2;
        }

        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        SpeedText.text = "AIRSPEED: " + Mathf.RoundToInt(player.CurrentSpeed * speedFudgeFactor).ToString();

        if (player.IsStalled)
        {
            if (nextStallBlink == -1)
            {
                nextStallBlink = Time.time + StallBlinkTime;
                StallText.enabled = true;
            } else if (Time.time > nextStallBlink)
            {
                StallText.enabled = !StallText.enabled;
                nextStallBlink = Time.time + StallBlinkTime;
            }
        } else
        {
            StallText.enabled = false;
            nextStallBlink = -1;
        }

        ScoreText.text = string.Format("LEVEL: {0,5}\nSCORE: {1,5}", ScoreManager.Instance.Level + 1, ScoreManager.Instance.Score);
    }

}
