using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_UIScore : MonoBehaviour
{
    G20_ScoreManager score;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        G20_ScoreManager.GetInstance().Base.ScoreChangedAction += ApplyScore;
        ApplyScore(0);
    }
    void ApplyScore(int _score)
    {
        scoreText.text = "" + _score;
    }
}
