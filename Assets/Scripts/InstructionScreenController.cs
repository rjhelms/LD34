using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();                                 // for in standalone build
        }
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
