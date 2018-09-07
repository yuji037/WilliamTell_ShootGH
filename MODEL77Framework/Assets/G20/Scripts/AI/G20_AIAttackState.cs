using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_AIAttackState : G20_AIState
{
    Action attackAction;
    public G20_AIAttackState(G20_AI _owner,Action attack_action) : base(_owner.enemy.anim.AnimSpeed/1.0f, _owner) { attackAction = attack_action; }
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
        }
        return null;
    }
}
