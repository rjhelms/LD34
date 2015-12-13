using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            // reset score lives & level, and go back to scene 0
            // in a just world this would redundant, but this is Ludum Dare
            ScoreManager.Instance.Score = 0;
            ScoreManager.Instance.Lives = 3;
            ScoreManager.Instance.Level = 0;

            // presumably the title screen is the first one
            SceneManager.LoadScene("MainScene");
        }
    }
}
