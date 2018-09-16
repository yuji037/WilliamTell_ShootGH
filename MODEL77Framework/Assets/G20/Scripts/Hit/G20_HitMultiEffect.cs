using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitMultiEffect : G20_HitAction
{
    [SerializeField] G20_EffectType[] effectss;
    [SerializeField] bool isCorrect=true;
    int currentNum = 0;
    public override void Execute(Vector3 hit_point)
    {
        var effect=G20_EffectManager.GetInstance().Create(effectss[currentNum], hit_point);
        //補正
        if(isCorrect)effect.transform.position = G20_PositionCorrector.Correct(effect.transform.position);
        if (currentNum < effectss.Length - 1)
        {
            currentNum++;
        }
    }
}
