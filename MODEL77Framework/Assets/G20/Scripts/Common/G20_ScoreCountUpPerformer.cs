using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_ScoreCountUpPerformer : G20_Singleton<G20_ScoreCountUpPerformer>
{
    public void StartCountUpScore(Text scoreText,int startScore,int endScore,float duration)
    {
        var changeInterval = duration/(float)(endScore - startScore);
        StartCoroutine(ScoreRoutine(scoreText,startScore,endScore, changeInterval));
    }
    public  void StartCountUpScore(Text scoreText,int startScore, System.Func<int> getPurposeScoreFunc, float scoreChangeInterval = 0.003f)
    {
        scoreText.text = startScore.ToString();
        StartCoroutine(ScoreRoutine(scoreText, startScore,getPurposeScoreFunc, scoreChangeInterval));
    }
    //毎フレーム目的スコアを取得してカウントアップ
    IEnumerator ScoreRoutine(Text scoreText,int startScore, System.Func<int> getPurPoseScoreFunc, float scoreChangeInterval = 0.003f)
    {
        float virtualCurrentScore = startScore;
        while (true)
        {
            //毎フレーム今のスコアを取得して、代入
            var purposeScore= getPurPoseScoreFunc();

            if (scoreChangeInterval != 0.0f)
            {
                if (OneCountUp(ref virtualCurrentScore, purposeScore,scoreChangeInterval))
                {
                    ApplyScoreToText(scoreText,(int)virtualCurrentScore);
                }
            }
            else
            {
                ApplyScoreToText(scoreText, purposeScore);
            }
            yield return null;
        }
    }

    //purposeScoreを目的のスコアとしてカウントアップ
    IEnumerator ScoreRoutine(Text scoreText, int startScore, int purposeScore, float scoreChangeInterval = 0.003f)
    {
        Debug.Log("Start" + startScore + "End" + purposeScore);
        float virtualCurrentScore = startScore;
        while (true)
        {
            if (scoreChangeInterval != 0.0f)
            {
                if (OneCountUp(ref virtualCurrentScore, purposeScore, scoreChangeInterval))
                {
                    ApplyScoreToText(scoreText, (int)virtualCurrentScore);
                }
            }
            else
            {
                ApplyScoreToText(scoreText, purposeScore);
            }
            yield return null;
        }
    }

    bool OneCountUp(ref float preScore,float currentScore,float scoreChangeInterval)
    {
        var sub = currentScore - (int)preScore;
        var addValue = (1.0f / scoreChangeInterval) * Time.deltaTime;
        if (sub > 0)
        {
            preScore += addValue;
            if ((int)preScore > currentScore)
            {
                preScore = currentScore;
            }
            return true;
        }
        else if (sub < 0)
        {
            preScore -= addValue;
            if ((int)preScore < currentScore)
            {
                preScore = currentScore;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    void ApplyScoreToText(Text scoreText, int score)
    {
        scoreText.text = "" + score;
    }
}
