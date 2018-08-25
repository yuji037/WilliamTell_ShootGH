using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_UIScore : MonoBehaviour {
    G20_Score score;
    [SerializeField]Text scoreText;

    private void Awake()
    {
        G20_Score.GetInstance().ScoreChangedAction+=ApplyScore;
    }
    void ApplyScore(int _score)
    {
        scoreText.text = ""+_score;
    }
}
