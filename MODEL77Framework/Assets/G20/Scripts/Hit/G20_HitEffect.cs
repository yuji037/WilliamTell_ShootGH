using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitEffect : G20_HitAction
{

    [SerializeField]G20_EffectType effectType;
    [SerializeField]bool isCorrect=true;
    public override void Execute(Vector3 hit_point)
    {
        var effect = G20_EffectManager.GetInstance().Create(effectType, hit_point);
        if(isCorrect)effect.transform.position = G20_PositionCorrector.Correct(effect.transform.position);
    }
    // 当たるたびにエフェクト変える場合は
    // これではなくG20_HitMultiEffectを使う
    public void ChangeEffectType(G20_EffectType effType) { effectType = effType; }
}
