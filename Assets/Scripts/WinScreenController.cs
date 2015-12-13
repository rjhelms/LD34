using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{

    public Text ScoreText;
    public Text InstructionText;

    public float WaitTime = 1f;

    private float ReadyTime;
    private bool isReady = false;

    // Use this for initialization
    void Start()
    {
        ScoreText.text = string.Format("FINAL SCORE {0,12}", ScoreManager.Instance.Score);
        ReadyTime = Time.time + WaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();                                 // for in standalone build
        }
        if (!isReady)
        {
            if (Time.time > ReadyTime)
            {
                InstructionText.gameObject.SetActive(true);
                isReady = true;
            }
        }
        else
        {

            if (Input.anyKeyDown)
            {
                // reset score lives & level, and go back to scene 0
                // in a just world this would redundant, but this is Ludum Dare
                ScoreManager.Instance.Score = 0;
                ScoreManager.Instance.Lives = 3;
                ScoreManager.Instance.Level = 0;

                // presumably the title screen is the first one
                SceneManager.LoadScene(0);
            }
        }
    }
}
