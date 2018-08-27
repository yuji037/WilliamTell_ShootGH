using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitClearApple : G20_HitAction {

    [SerializeField] float colliderExpansionRate = 10.0f;

    public override void Execute(Vector3 hit_point)
    {
        GetComponent<SphereCollider>().radius *= colliderExpansionRate;
    }
}
