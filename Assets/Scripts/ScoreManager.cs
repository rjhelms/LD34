using UnityEngine;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>
{
    protected ScoreManager() { }

    public int Score;
    public int Lives;
}
