using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitLowerScore : G20_HitAction {
    [SerializeField]G20_Enemy lowerTarget;
    [SerializeField] int oneHitLowerValue = 20;
    public override void Execute(Vector3 hit_point)
    {
        lowerTarget.ScoreCaluclator.LowerHaveScore(oneHitLowerValue);
    }
}
