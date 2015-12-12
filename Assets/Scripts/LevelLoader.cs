using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    public TextAsset[] LevelFiles;
    public TextAsset[] LevelText;

    public GameObject LevelTextPrefab;
    public Canvas LevelTextCanvas;

	// Use this for initialization
	void Start () {
        LoadLevelText(ScoreManager.Instance.Level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void LoadLevelText(int Level)
    {
        Debug.Log(LevelText[0]);
        CSVHelper csv = new CSVHelper(LevelText[0].ToString(), ",");
        foreach (string[] line in csv)
        {
            Debug.Log(line.Length);
            GameObject newTextGameObject = GameObject.Instantiate(LevelTextPrefab);
            Text newText = newTextGameObject.GetComponent<Text>();
            newText.text = line[0];
            newText.transform.position = new Vector2(float.Parse(line[1]), float.Parse(line[2]));
            newText.transform.parent = LevelTextCanvas.transform;
        }
    }
}
