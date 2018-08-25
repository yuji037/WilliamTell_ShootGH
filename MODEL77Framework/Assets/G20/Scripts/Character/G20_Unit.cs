using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_Unit : MonoBehaviour {
    [SerializeField] int hp;
    public int HP
    {
        get { return hp;}
    }
    public bool IsInvincible=false;
    public event Action<G20_Unit> deathActions;
    public event Action<G20_Unit> recvDamageActions;
    bool isStartDeath = false;
    public void RecvDamage(int damage_value)
    {
        if (0 >= hp|| IsInvincible) return;
        hp -= Mathf.Abs(damage_value);
        if (recvDamageActions!=null)recvDamageActions(this);
        if (0>=hp)
        {
            ExecuteDeathAction();
        }
    }
    //deathactionsが実際に実行されるのは1回のみ
    public void ExecuteDeathAction()
    {
        if (isStartDeath) return;
        if (deathActions != null) deathActions(this);
        isStartDeath = true;
    }
}
