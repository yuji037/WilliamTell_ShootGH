using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_AIAttackState : G20_AIState
{
    Action attackAction;
    public G20_AIAttackState(float end_time, G20_AI _owner,Action attack_action) : base(end_time, _owner) { attackAction = attack_action; }
    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        owner.enemy.anim.Attack();
    }

    protected override G20_AIState Update()
    {
        if (CheckOver())
        {
            if(attackAction!=null)attackAction();
            return new G20_AIDeathState(1.0f,owner);
        }
        return null;
    }
}
