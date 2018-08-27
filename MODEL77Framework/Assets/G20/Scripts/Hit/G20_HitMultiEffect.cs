using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitMultiEffect : G20_HitAction
{
    [SerializeField] G20_EffectType[] effectss;
    int currentNum = 0;
    public override void Execute(Vector3 hit_point)
    {
        G20_EffectManager.GetInstance().Create(effectss[currentNum], hit_point);
        if (currentNum < effectss.Length - 1)
        {
            currentNum++;
        }
    }
}
