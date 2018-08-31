using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//AIの状態を制御するclass
public class G20_StateController {
    G20_AI owner;
    G20_AIState currentState;
    public G20_StateController(G20_AI _owner)
    {
        owner = _owner;
    }
    public void Start()
    {
        //0だったら怯まないように設定
        if (owner.enemy.hirumiTime>0f)
        {
            owner.enemy.recvDamageActions += _ => Falter(owner.enemy.hirumiTime/owner.enemy.Speed);
        }
        owner.enemy.deathActions += _ => Death();
        Run();
    }
    public void Update()
    {
        if (currentState != null)
        {
            var state = currentState.BaseUpdate();
            if (state != null)
            {
                ChangeState(state);
            }
        }                       
    }
    public void Run()
    {
        if (currentState is G20_AIFalterState) return;
        ChangeState(new G20_AIRunState(1.0f, owner));
    }
    //attack_time秒後に、attack_actionを実行し死ぬ
    public void Attack(float attack_time,System.Action attack_action)
    {
        ChangeState(new G20_AIAttackState(attack_time, owner, attack_action));
    }
    public void Falter(float hirumi_time)
    {
        //attack中は怯まない
        if (currentState is G20_AIAttackState) return;
        if (currentState is G20_AIFalterState)
        {
            ChangeState(new G20_AIFalterState(hirumi_time, owner, ((G20_AIFalterState)currentState).preState));
        }
        else
        {
            ChangeState(new G20_AIFalterState(hirumi_time, owner, currentState));
        }
    }
    public void Death()
    {
        ChangeState(new G20_AIDeathState(1.0f, owner));
    }
    void ChangeState(G20_AIState ai_state)
    {
        if (currentState != null) currentState.OnEnd();
        currentState = ai_state;
        currentState.OnStart();
    }
}
