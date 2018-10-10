using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_Unit : MonoBehaviour
{
    public enum G20_DamageType
    {
        Player,Enemy,System
    }
    [SerializeField] protected int hp;
    public int HP
    {
        get { return hp; }
    }

    public bool IsInvincible = false;
    public event Action<G20_Unit,G20_DamageType> deathActions;
    public event Action<G20_Unit> OnDestroyAction;
    public event Action<G20_Unit,G20_DamageType> recvDamageActions;
    [SerializeField] G20_ScoreCalculator scoreCaluclator;
    bool isStartDeath = false;
    protected void UpChainCount(G20_DamageType damageType)
    {
        if (damageType == G20_DamageType.Player)
        {
            G20_ChainCounter.GetInstance().UpChainCount();
        }
    }
    public void RecvDamage(int damage_value,G20_DamageType damageType)
    {
        if (0 >= hp || IsInvincible) return;
        damage_value = Mathf.Clamp(damage_value,0,hp);
        scoreCaluclator.CalcAndAddScore(damage_value);
        hp -= damage_value;
        if (recvDamageActions != null) recvDamageActions(this,damageType);
        if (0 >= hp)
        {
            ExecuteDeathAction(damageType);
        }
    }
    private void OnDestroy()
    {
        if (OnDestroyAction != null)
        {
            OnDestroyAction(this);
        }
    }
    //deathactionsが実際に実行されるのは1回のみ
    public void ExecuteDeathAction(G20_DamageType damageType)
    {
        if (isStartDeath) return;
        if (deathActions != null) deathActions(this,damageType);
        isStartDeath = true;
    }
}
