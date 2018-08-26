using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIDeathState : G20_AIState
{
    public G20_AIDeathState(float end_time, G20_AI _owner) : base(end_time, _owner) { }


    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        owner.isPouse = true;
        if (owner.enemy.HP > 0)
        {
            owner.enemy.anim.Suicide();
        }
        else
        {
            owner.enemy.anim.Death();
        }
    }

    protected override G20_AIState Update()
    {
        if (CheckOver())
        {
            GameObject.Destroy(owner.enemy.gameObject);
        }
        return null;
    }
}
