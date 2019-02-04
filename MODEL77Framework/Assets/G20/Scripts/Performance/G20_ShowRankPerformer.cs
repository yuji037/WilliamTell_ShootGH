using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class G20_ShowRankPerformer : MonoBehaviour
{
    [System.Serializable]
    struct ResultParam
    {
      public Color color;
      public string text;
      public int scoreCondition;
    }

    [System.Serializable]
    struct ResultParams
    {
       public List<ResultParam> resultParams;
    }

    [SerializeField] Text baseScore;
    [SerializeField] Text chainValue;
    [SerializeField] Text chainScore;
    [SerializeField] Text hitRateValue;
    [SerializeField] Text hitRateScore;
    [SerializeField] Text totalScore;
    [SerializeField] Animator showRankAnim;
    [SerializeField] Text rankText;

    [SerializeField]List<ResultParams> resultParams;

    [SerializeField] float chainCountUpDuration=3.0f;
    [SerializeField] float hitRateCountUpDuration=3.0f;

    int currentTotalScore = 0;
    public void StartPerformance()
    {
        StartCoroutine(PerformanceRoutine());
    }

    
    IEnumerator PerformanceRoutine()
    {

        //トータルスコア瞬時表示
        currentTotalScore += G20_ScoreManager.GetInstance().GetBaseScore();
        totalScore.text = currentTotalScore.ToString();
        var baseScoreStr = G20_ScoreManager.GetInstance().GetBaseScore().ToString();
        baseScore.text = baseScoreStr;
        yield return new WaitForSeconds(1.0f);

        //チェイン数、チェインボーナス、トータルスコア、カウントアップ
        var preScore = currentTotalScore;
        currentTotalScore += G20_ScoreManager.GetInstance().GetMaxChainBonus();
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(chainValue, 0, G20_ChainCounter.GetInstance().MaxChainCount, chainCountUpDuration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(chainScore, 0, G20_ScoreManager.GetInstance().GetMaxChainBonus(), chainCountUpDuration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(totalScore, preScore, currentTotalScore, chainCountUpDuration);
        yield return new WaitForSeconds(chainCountUpDuration + 1.0f);

        //命中率、命中率ボーナス、トータルスコア、カウントアップ
        preScore = currentTotalScore;
        currentTotalScore += G20_ScoreManager.GetInstance().GetHitRateBonus();
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(hitRateValue, 0, (int)(G20_BulletShooter.GetInstance().HitRate * 100), hitRateCountUpDuration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(hitRateScore, 0, G20_ScoreManager.GetInstance().GetHitRateBonus(), hitRateCountUpDuration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(totalScore, preScore, currentTotalScore, hitRateCountUpDuration);
        yield return new WaitForSeconds(hitRateCountUpDuration);

        //RANK表示ドーン
        ShowRank();
        showRankAnim.CrossFade("ShowRank", 0f);

    }

    IEnumerator FadeRoutine(Text text, float take_time, bool is_plus)
    {
        int multipliValue = 1;
        if (!is_plus) multipliValue = -1;
        for (float t = 0; t < take_time; t += Time.deltaTime)
        {
            text.color += multipliValue * new Color(0, 0, 0, (1.0f / take_time) * Time.deltaTime);
            yield return null;
        }

    }

    void ShowRank()
    {
        var sumScore = G20_ScoreManager.GetInstance().GetSumScore();
        var resultParam = resultParams[G20_GameManager.GetInstance().gameDifficulty];
        resultParam.resultParams.Sort((a,b)=>b.scoreCondition-a.scoreCondition);
        foreach (var i in resultParam.resultParams)
        {
            if (sumScore >= i.scoreCondition)
            {
                rankText.text = i.text;
                rankText.color = i.color;
                return;
            }
        }
    }
}

