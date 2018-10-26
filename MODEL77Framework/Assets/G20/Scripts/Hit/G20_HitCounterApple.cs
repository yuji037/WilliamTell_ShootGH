using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitCounterApple : G20_HitAction {
    
    public override void Execute(Vector3 hit_point)
    {
        G20_BulletAppleCreator.GetInstance().Create(hit_point);
    }
}
