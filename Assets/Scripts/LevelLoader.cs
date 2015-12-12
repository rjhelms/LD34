using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class LevelLoader : MonoBehaviour
{

    public TextAsset[] Levels;
    public string LevelFolder;
    public string PrefabFolder;
    public GameObject LevelTextPrefab;
    public Transform LevelTextCanvas;
    public Transform LevelTilesParent;

    public int GridSize = 16;

    private TextAsset levelTiles;
    private TextAsset levelTilePrefabs;
    private TextAsset levelText;

    // Use this for initialization
    void Start()
    {
        string[] levelDef = Regex.Split(Levels[ScoreManager.Instance.Level].ToString(), System.Environment.NewLine);
        levelTiles = Instantiate(Resources.Load(LevelFolder + levelDef[0], typeof(TextAsset)) as TextAsset);
        levelTilePrefabs = Instantiate(Resources.Load(LevelFolder + levelDef[1], typeof(TextAsset)) as TextAsset);
        levelText = Instantiate(Resources.Load(LevelFolder + levelDef[2], typeof(TextAsset)) as TextAsset);
        LoadLevelTiles(levelTiles, levelTilePrefabs);
        LoadLevelText(levelText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadLevelTiles(TextAsset tiles, TextAsset prefabs)
    {
        string[] prefabDef = Regex.Split(prefabs.ToString(), System.Environment.NewLine);
        GameObject[] prefabObjects = new GameObject[prefabDef.Length];
        for (int i = 0; i < prefabDef.Length; i++)
        {
            prefabObjects[i] = Resources.Load(PrefabFolder + prefabDef[i], typeof(GameObject)) as GameObject;
        }

        CSVHelper csv = new CSVHelper(tiles.ToString(), ",");
        Debug.Log(csv.Count);
        int lineNum = 0;
        foreach (string[] line in csv)
        {
            int yCoordinate = (csv.Count - (lineNum + 1)) * GridSize;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != string.Empty)
                {
                    Debug.Log(line[i]);
                    GameObject newObject = GameObject.Instantiate(prefabObjects[int.Parse(line[i])]);
                    newObject.transform.position = new Vector3(i * GridSize, yCoordinate, 1);
                    newObject.transform.SetParent(LevelTilesParent, true);
                }
            }
            lineNum++;
        }
    }

    private void LoadLevelText(TextAsset text)
    {
        CSVHelper csv = new CSVHelper(text.ToString(), ",");
        foreach (string[] line in csv)
        {
            Debug.Log(line.Length);
            GameObject newTextGameObject = GameObject.Instantiate(LevelTextPrefab);
            Text newText = newTextGameObject.GetComponent<Text>();
            newText.text = line[0];
            newText.transform.position = new Vector2(float.Parse(line[1]), float.Parse(line[2]));
            newText.transform.SetParent(LevelTextCanvas.transform, true);
        }
    }
}
