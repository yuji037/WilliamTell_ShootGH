using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIFalterState : G20_AIState
{
    public G20_AIState preState;
    public G20_AIFalterState(float end_timer,G20_AI _owner,G20_AIState pre_state) : base(end_timer, _owner) { preState = pre_state; }
    public override void OnEnd()
    {
        Debug.Log("停止解除");
        owner.isPouse = false;
    }

    public override void OnStart()
    {
        Debug.Log("停止開始");
        owner.isPouse = true;
        owner.enemy.anim.Falter();
    }

    protected override G20_AIState Update()
    {
        if (CheckOver())
        {
            return  preState;
        }
        return null;
    }
}
