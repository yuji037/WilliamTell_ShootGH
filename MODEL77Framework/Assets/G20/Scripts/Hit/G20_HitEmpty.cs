using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//scriptでActionをSetする
public class G20_HitEmpty : G20_HitAction {
    public event Action hitAction;
    public override void Execute(Vector3 hit_point)
    {
        if (hitAction != null) hitAction();
    }
    private void OnDestroy()
    {
        hitAction = null;
    }
}
