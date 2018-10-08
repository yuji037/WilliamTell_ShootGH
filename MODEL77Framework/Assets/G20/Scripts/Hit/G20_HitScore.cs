using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ヒットしたらスコアを1回だけ加算するクラス
public class G20_HitScore : G20_HitAction
{
    [SerializeField] int scoreValue;
    //この数値が0になるまでスコアは加算されない
    [SerializeField] int armor;
    [SerializeField] int canHitValue = 1;
    public override void Execute(Vector3 hit_point)
    {
        if (canHitValue <=0) return;
        if (!RecvArmor())
        {
            G20_EffectManager.GetInstance().Create(G20_EffectType.PLUS_ONE_SCORE,hit_point);
            G20_ScoreManager.GetInstance().Base.AddScore(scoreValue);
            canHitValue--;
        }
    }
    //アーマーで受けたらtrueを返す
    bool RecvArmor()
    {
        if (armor > 0)
        {
            armor -= 1;
            return true;
        }
        return false;
    }
}
