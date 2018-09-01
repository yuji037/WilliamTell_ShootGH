using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIIdleState : G20_AIState {
    public G20_AIIdleState(float end_timer, G20_AI _owner) : base(end_timer, _owner) { }
    public override void OnEnd()
    {

    }

    public override void OnStart()
    {
        owner.enemy.anim.Idle();
    }

    protected override G20_AIState Update()
    {
        return null;
    }
}
