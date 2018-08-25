using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitBomb : G20_HitAction {
    public override void Execute(Vector3 hit_point)
    {
        G20_EnemyCabinet.GetInstance().DamageAllEnemys(3);
    }
}