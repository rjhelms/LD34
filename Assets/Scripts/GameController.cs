using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Material RenderTexture;
    public Camera WorldCamera;

    [Header("HUD UI Elements")]
    public Text SpeedText;
    public Text StallText;
    public Text ScoreText;
    public Text LivesText;
    public float StallBlinkTime;

    [Header("Level Clear Elements")]
    public Canvas ClearCanvas;
    public Text TotalText;
    public Text SprayedText;
    public Text PenaltyText;
    public Text ClearScoreText;
    public Text AnyKeyText;
    public float AdvanceTime;
    public int LevelClearUIState;

    [Header("Resolution")]
    public int TargetX = 160;
    public int TargetY = 200;
    
    [Header("Audio")]
    public AudioClip EngineNoise;
    public AudioClip StallNoise;
    public AudioClip CrashNoise;
    public AudioClip CropDustNoise;
    public AudioClip WinNoise;
    public AudioClip MusicMainTheme;
    public GameObject MusicPlayerPrefab;

    [Header("Game State")]
    public bool Running = false;
    public bool HasWonLevel = false;
    public bool HasDied = false;
    public bool IsCrashing = false;
    public bool Starting = true;
    public int LevelX = 0;

    [Header("Score Balance")]
    public int DustCropScore = 100;
    public int LandingScore = 50;
    public int MissedCropPenalty = 100;
    public int CrashPenalty = 500;

    private PlayerController player;
    private float pixelRatioAdjustment;
    private float speedFudgeFactor = 33.3f;

    private float nextStallBlink = -1;
    private bool isPlayingCropNoise;
    private float nextClearStateTime;

    private int totalCropFields;
    private int unsprayedCropFields;

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

        GameObject musicPlayer;
        if (!GameObject.FindGameObjectWithTag("Music"))
        {
            musicPlayer = Instantiate(MusicPlayerPrefab);
            DontDestroyOnLoad(musicPlayer);
        }
        else
        {
            musicPlayer = GameObject.FindGameObjectWithTag("Music");
        }

        AudioSource musicPlayerSource = musicPlayer.GetComponent<AudioSource>();
        if (musicPlayerSource.clip != MusicMainTheme)
        {
            musicPlayerSource.Stop();
            musicPlayerSource.clip = MusicMainTheme;
            musicPlayerSource.Play();
        }

        Starting = true;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        UpdateUI();
        if (HasWonLevel)
        {
            if (LevelClearUIState < 7)
            {
                if (Time.time > nextClearStateTime)
                {
                    AdvanceLevelClearState();
                }
            }
            else {
                if (Input.anyKey)
                {
                    ScoreManager.Instance.Level++;

                    if (ScoreManager.Instance.Level >= gameObject.GetComponent<LevelLoader>().Levels.Length)
                    {
                        SceneManager.LoadScene("WinScreen");
                    }
                    else
                    {
                        SceneManager.LoadScene("MainScene");
                    }
                }
            }
        }
        else if (IsCrashing)
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
                    SceneManager.LoadScene("GameOver");
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
        else if (Starting)
        {
            if (Input.anyKeyDown)
            {
                Starting = false;
                Running = true;
                gameSoundSource.Play();
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
            ScoreManager.Instance.Score -= CrashPenalty;
            if (ScoreManager.Instance.Score < 0)
            {
                ScoreManager.Instance.Score = 0;
            }

            isPlayingCropNoise = false;
            IsCrashing = true;
            gameSoundSource.pitch = 1;
            gameSoundSource.Stop();
            gameSoundSource.PlayOneShot(CrashNoise);
        }
    }

    public void PlayCropNoise()
    {
        isPlayingCropNoise = true;
        ScoreManager.Instance.Score += DustCropScore;
        gameSoundSource.pitch = 1;
        gameSoundSource.Stop();
        gameSoundSource.PlayOneShot(CropDustNoise);
    }

    public void WinLevel()
    {
        if (!IsCrashing)
        {
            ScoreManager.Instance.Score += LandingScore;
            Running = false;
            gameSoundSource.pitch = 1;
            gameSoundSource.Stop();
            gameSoundSource.PlayOneShot(WinNoise);
            HasWonLevel = true;
            nextClearStateTime = Time.time + AdvanceTime;
            GameObject[] cropFields = GameObject.FindGameObjectsWithTag("Crop");
            totalCropFields = cropFields.Length;
            unsprayedCropFields = 0;

            for (int i = 0; i < cropFields.Length; i++)
            {
                if (!cropFields[i].GetComponent<Crop>().IsLive)
                {
                    unsprayedCropFields++;
                }
            }
        }
    }

    private void AdvanceLevelClearState()
    {
        switch (LevelClearUIState)
        {
            case 0:
                ClearCanvas.gameObject.SetActive(true);
                LevelClearUIState++;
                nextClearStateTime = Time.time + AdvanceTime;
                break;
            case 1:
                TotalText.text = string.Format("TOTAL FIELDS {0,11}", totalCropFields);
                TotalText.gameObject.SetActive(true);
                LevelClearUIState++;
                nextClearStateTime = Time.time + AdvanceTime;
                break;
            case 2:
                SprayedText.text = string.Format("FIELDS SPRAYED {0, 9}", totalCropFields - unsprayedCropFields);
                SprayedText.gameObject.SetActive(true);
                LevelClearUIState++;
                nextClearStateTime = Time.time + AdvanceTime;
                break;
            case 3:
                PenaltyText.text = string.Format("PENALTY FOR\nUNSPRAYED FIELDS {0, 7}", unsprayedCropFields * MissedCropPenalty);
                PenaltyText.gameObject.SetActive(true);
                LevelClearUIState++;
                nextClearStateTime = Time.time + AdvanceTime;
                break;
            case 4:
                ClearScoreText.text = string.Format("SCORE {0,18}", ScoreManager.Instance.Score);
                ClearScoreText.gameObject.SetActive(true);
                LevelClearUIState++;
                if (unsprayedCropFields == 0) LevelClearUIState++;  // no need to process the penalty step if there is none
                nextClearStateTime = Time.time + AdvanceTime;
                break;
            case 5:
                ScoreManager.Instance.Score -= unsprayedCropFields * MissedCropPenalty;
                if (ScoreManager.Instance.Score < 0) ScoreManager.Instance.Score = 0;

                ClearScoreText.text = string.Format("SCORE {0,18}", ScoreManager.Instance.Score);
                PenaltyText.text = string.Format("PENALTY FOR\nUNSPRAYED FIELDS {0, 7}", 0);
                LevelClearUIState++;
                nextClearStateTime = Time.time + AdvanceTime;
                break;
            case 6:
                AnyKeyText.gameObject.SetActive(true);
                LevelClearUIState++;
                break;
        }
    }
}
