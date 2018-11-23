using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_UIScore : MonoBehaviour
{
    G20_ScoreManager score;
    [SerializeField] Text scoreText;
    [SerializeField] float scoreChangeInterval = 0.1f;
    float virtualCurrentScore = 0;
    private void Start()
    {
        scoreText.text = "0";
        StartCoroutine(ScoreRoutine());
    }
    IEnumerator ScoreRoutine()
    {
        while (true)
        {
            var ScoreValue = G20_ScoreManager.GetInstance().GetBaseScore();
            var sub = ScoreValue - (int)virtualCurrentScore;
            var addValue = (1.0f / scoreChangeInterval) * Time.deltaTime;
            if (sub > 0)
            {
                virtualCurrentScore += addValue;
                if ((int)virtualCurrentScore > ScoreValue)
                {
                    virtualCurrentScore = ScoreValue;
                }
                scoreText.text = "" + (int)virtualCurrentScore;
            }
            else if (sub < 0)
            {
                virtualCurrentScore -= addValue;
                if ((int)virtualCurrentScore < ScoreValue)
                {
                    virtualCurrentScore = ScoreValue;
                }
                scoreText.text = "" + (int)virtualCurrentScore;
            }
            else
            {
                //何もしない
            }

            yield return null;
        }
    }
}
