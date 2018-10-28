using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ownerが受けたダメージによってスコアを計算し、加算するclass
[System.Serializable]
public class G20_ScoreCalculator
{
    //1ダメージ毎にプレイヤーが獲得するスコア
    [SerializeField] int oneDamageStealValue;
    //持ちスコア
    [SerializeField] private int haveScore;
    [SerializeField] Transform scoreEffectTransform;
    public int HaveScore
    {
        get { return haveScore; }
    }
    public void LowerHaveScore(int lowerValue)
    {
        haveScore-= lowerValue;
        if (haveScore<=0)
        {
            haveScore = 0;
        }
    }
    int StealHaveScore(int stealScore)
    {
        //持ちスコア以上取れないように制限
        stealScore = Mathf.Clamp(stealScore,0,haveScore);
        //持ちスコアから引く
        haveScore -= stealScore;
        return stealScore;

    }
    public void CalcAndAddScore(int _damage)
    {
  
        var score = StealHaveScore(oneDamageStealValue*_damage);
        if (score <= 0) return;
        G20_ScoreManager.GetInstance().Base.AddScore(score);
        var bonusScore = G20_ChainCounter.GetInstance().GetOneTimeBonusScore();
        G20_ScoreManager.GetInstance().Bonus.AddScore(bonusScore);
        var obj = G20_EffectManager.GetInstance().Create(G20_EffectType.PLUS_ONE_SCORE, scoreEffectTransform.position);
        obj.GetComponent<TextMesh>().text = "+" + (score+bonusScore);

    }
}
