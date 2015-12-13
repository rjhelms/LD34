using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Material RenderTexture;
    public Camera WorldCamera;
    public bool Running;

    public Text SpeedText;
    public Text StallText;
    public Text ScoreText;
    public Text LivesText;

    public int TargetX = 160;
    public int TargetY = 200;

    public float StallBlinkTime;

    public AudioClip EngineNoise;
    public AudioClip StallNoise;
    public AudioClip CrashNoise;
    public AudioClip CropDustNoise;
    public AudioClip WinNoise;

    public bool HasWonLevel = false;
    public bool HasDied = false;
    public bool IsCrashing = false;

    public int LevelX = 0;

    private PlayerController player;
    private float pixelRatioAdjustment;
    private float speedFudgeFactor = 33.3f;

    private float nextStallBlink = -1;
    private bool isPlayingCropNoise;
    private float nextLevelTime;
    
    private AudioSource gameSoundSource;

    // Use this for initialization
    void Start()
    {
        pixelRatioAdjustment = (float)TargetX / (float)TargetY;
        if (pixelRatioAdjustment <= 1)
        {
            RenderTexture.mainTextureScale = new Vector2(pixelRatioAdjustment, 1);
            RenderTexture.mainTextureOffset = new Vector2((1 - pixelRatioAdjustment) / 2, 0);
            WorldCamera.orthographicSize = TargetY / 2;
        }
        else
        {
            pixelRatioAdjustment = 1f / pixelRatioAdjustment;
            RenderTexture.mainTextureScale = new Vector2(1, pixelRatioAdjustment);
            RenderTexture.mainTextureOffset = new Vector2(0, (1 - pixelRatioAdjustment) / 2);
            WorldCamera.orthographicSize = TargetX / 2;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        LevelX = GetComponent<LevelLoader>().LevelX;
        gameSoundSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ScoreManager.Instance.Level++;
            SceneManager.LoadScene("MainScene");
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ScoreManager.Instance.Lives++;
        }

        UpdateUI();
        if (HasWonLevel)
        {
            if (Time.time > nextLevelTime)
            {
                ScoreManager.Instance.Level++;
                SceneManager.LoadScene("MainScene");
            }
        }
        if (IsCrashing)
        {
            if (!gameSoundSource.isPlaying)
            {
                IsCrashing = false;
                HasDied = true;
                if (ScoreManager.Instance.Lives > 0)
                {
                    ScoreManager.Instance.Lives--;
                    SceneManager.LoadScene("MainScene");
                }
                else
                {
                    Debug.Log("Game over!");
                }
            }
        }
        else if (Running)
        {
            if (isPlayingCropNoise)
            {
                if (!gameSoundSource.isPlaying)
                {
                    gameSoundSource.clip = EngineNoise; // don't know why this is neccessary
                    gameSoundSource.Play();             // should trigger below but it doesn't
                    isPlayingCropNoise = false;
                }
            }
            else if (player.IsStalled)
            {
                gameSoundSource.pitch = 1;
                if (gameSoundSource.clip != StallNoise)
                {
                    gameSoundSource.clip = StallNoise;
                    gameSoundSource.loop = true;
                    gameSoundSource.Play();
                }

            }
            else if (!player.IsStalled)
            {
                gameSoundSource.pitch = player.CurrentSpeed * .75f;
                if (gameSoundSource.clip != EngineNoise)
                {
                    gameSoundSource.clip = EngineNoise;
                    gameSoundSource.loop = true;
                    gameSoundSource.Play();
                }
            }

        }
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
            }
            else if (Time.time > nextStallBlink)
            {
                StallText.enabled = !StallText.enabled;
                nextStallBlink = Time.time + StallBlinkTime;
            }
        }
        else
        {
            StallText.enabled = false;
            nextStallBlink = -1;
        }

        ScoreText.text = string.Format("LEVEL: {0,5}\nSCORE: {1,5}", ScoreManager.Instance.Level + 1, ScoreManager.Instance.Score);
        LivesText.text = string.Format("LIVES: {0}", ScoreManager.Instance.Lives);
    }

    public void Crash()
    {
        if (!HasWonLevel)
        {
            Running = false;
            IsCrashing = true;
            gameSoundSource.pitch = 1;
            gameSoundSource.Stop();
            gameSoundSource.PlayOneShot(CrashNoise);
        }
    }

    public void PlayCropNoise()
    {
        isPlayingCropNoise = true;
        gameSoundSource.pitch = 1;
        gameSoundSource.Stop();
        gameSoundSource.PlayOneShot(CropDustNoise);
    }

    public void WinLevel()
    {
        if (!IsCrashing)
        {
            Running = false;
            gameSoundSource.pitch = 1;
            gameSoundSource.Stop();
            gameSoundSource.PlayOneShot(WinNoise);
            HasWonLevel = true;
            nextLevelTime = Time.time + 2;
        }
    }
}
