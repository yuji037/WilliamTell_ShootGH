using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitSE : G20_HitAction {

    [SerializeField]
    G20_SEType seType;

    public override void Execute(Vector3 hit_point)
    {
        G20_SEManager.GetInstance().Play(seType, transform.position);
    }
}
