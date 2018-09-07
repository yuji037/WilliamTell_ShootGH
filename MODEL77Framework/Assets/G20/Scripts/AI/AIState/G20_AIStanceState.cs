﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIStanceState : G20_AIState
{
    public G20_AIStanceState(G20_AI _owner) : base(_owner.enemy.anim.AnimSpeed / 1.0f, _owner) { }
    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        owner.enemy.anim.AttackStance();
    }

    protected override G20_AIState Update()
    {
        return null;
    }
}
