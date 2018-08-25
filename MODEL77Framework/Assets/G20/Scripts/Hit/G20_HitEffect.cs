using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitEffect : G20_HitAction {

    [SerializeField]
    G20_EffectType effectType;

    public override void Execute(Vector3 hit_point)
    {
        G20_EffectManager.GetInstance().Create(effectType, hit_point);
    }
}
