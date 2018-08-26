using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_Unit : MonoBehaviour {
    [SerializeField] protected int hp;
    public int HP
    {
        get { return hp;}
    }
    public bool IsInvincible=false;
    public event Action<G20_Unit> deathActions;
    public event Action<G20_Unit> recvDamageActions;
    bool isStartDeath = false;
    public void RecvDamage(int damage_value,G20_DamageType damage_type)
    {
        if (0 >= hp|| IsInvincible) return;
        uRecvDamage(damage_value,damage_type);
        if (recvDamageActions!=null)recvDamageActions(this);
        if (0>=hp)
        {
            ExecuteDeathAction();
        }
    }
    //子classによってダメージの値を変更できる、仮想関数
    protected virtual void uRecvDamage(int damage_value, G20_DamageType damage_type)
    {
        hp -= Mathf.Abs(damage_value);
    }
    //deathactionsが実際に実行されるのは1回のみ
    public void ExecuteDeathAction()
    {
        if (isStartDeath) return;
        if (deathActions != null) deathActions(this);
        isStartDeath = true;
    }
}
