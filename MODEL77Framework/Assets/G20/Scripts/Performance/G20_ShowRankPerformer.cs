using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_ShowRankPerformer : MonoBehaviour
{
    [SerializeField] Text baseScore;
    [SerializeField] Text chainValue;
    [SerializeField] Text chainScore;
    [SerializeField] Text hitRateValue;
    [SerializeField] Text hitRateScore;
    [SerializeField] Text totalScore;
    [SerializeField] Animator showRankAnim;
    [SerializeField] Text rankText;
    int currentTotalScore = 0;
    public void StartPerformance()
    {
        StartCoroutine(PerformanceRoutine());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartPerformance();
        }
    }
    //IEnumerator PerformanceRoutine()
    //{

    //    //トータルスコア瞬時表示
    //    totalScore.text = currentTotalScore.ToString();
    //    currentTotalScore += G20_ScoreManager.GetInstance().GetBaseScore();
    //    var baseScoreStr = G20_ScoreManager.GetInstance().GetBaseScore().ToString();
    //    baseScore.text = baseScoreStr;

    //    //チェイン数、チェインボーナス、トータルスコア、カウントアップ
    //    Debug.Log("チェイン数、チェインボーナス、トータルスコア、カウントアップ");
    //    float duration = 2.0f;
    //    var preScore = currentTotalScore;
    //    currentTotalScore += 3400;
    //    G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(chainValue, 0, 11, duration);
    //    G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(chainScore, 0, 3400, duration);
    //    G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(totalScore, preScore, currentTotalScore, duration);
    //    yield return new WaitForSeconds(duration+0.5f);

    //    //命中率、命中率ボーナス、トータルスコア、カウントアップ
    //    Debug.Log("命中率、命中率ボーナス、トータルスコア、カウントアップ");
    //    preScore = currentTotalScore;
    //    currentTotalScore += 5200;
    //    G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(hitRateValue, 0, (int)(85), duration);
    //    G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(hitRateScore, 0, 5200, duration);
    //    G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(totalScore, preScore, currentTotalScore, duration);
    //    yield return new WaitForSeconds(duration);

    //    //RANK表示ドーン
    //    rankText.text = CalcRank();
    //    showRankAnim.CrossFade("ShowRank",0f);
    //}
    IEnumerator PerformanceRoutine()
    {

        //トータルスコア瞬時表示
        totalScore.text = currentTotalScore.ToString();
        currentTotalScore += G20_ScoreManager.GetInstance().GetBaseScore();
        var baseScoreStr = G20_ScoreManager.GetInstance().GetBaseScore().ToString();
        baseScore.text = baseScoreStr;

        //チェイン数、チェインボーナス、トータルスコア、カウントアップ
        Debug.Log("チェイン数、チェインボーナス、トータルスコア、カウントアップ");
        float duration = 2.0f;
        var preScore = currentTotalScore;
        currentTotalScore += G20_ScoreManager.GetInstance().GetMaxChainBonus();
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(chainValue, 0, G20_ChainCounter.GetInstance().MaxChainCount, duration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(chainScore, 0, G20_ScoreManager.GetInstance().GetMaxChainBonus(), duration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(totalScore, preScore, currentTotalScore, duration);
        yield return new WaitForSeconds(duration);

        //命中率、命中率ボーナス、トータルスコア、カウントアップ
        Debug.Log("命中率、命中率ボーナス、トータルスコア、カウントアップ");
        preScore = currentTotalScore;
        currentTotalScore += G20_ScoreManager.GetInstance().GetHitRateBonus();
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(hitRateValue, 0, (int)(G20_BulletShooter.GetInstance().HitRate * 100), duration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(hitRateScore, 0, G20_ScoreManager.GetInstance().GetHitRateBonus(), duration);
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(totalScore, preScore, currentTotalScore, duration);
        yield return new WaitForSeconds(duration);

        //RANK表示ドーン
        rankText.text = CalcRank();
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

    string CalcRank()
    {
        var sumScore = G20_ScoreManager.GetInstance().GetSumScore();
        if (sumScore>=15000)
        {
            return "S";
        }else if(sumScore>=13000)
        {
            return "A";
        }else if (sumScore >= 11000)
        {
            return "B";
        }else
        {
            return "C";
        }
        
    }
}

