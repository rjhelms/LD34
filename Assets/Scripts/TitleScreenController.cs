using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{

    public GameObject MusicPlayerPrefab;
    public AudioClip MusicMainTheme;

    // Use this for initialization
    void Start()
    {
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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.anyKeyDown)
        {
            // reset score lives & level, and go back to scene 0
            // in a just world this would redundant, but this is Ludum Dare
            ScoreManager.Instance.Score = 0;
            ScoreManager.Instance.Lives = 3;
            ScoreManager.Instance.Level = 0;

            SceneManager.LoadScene("InstructionScreen");
        }
    }
}
