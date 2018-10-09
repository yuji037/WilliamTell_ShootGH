using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_Unit : MonoBehaviour
{
    [SerializeField] protected int hp;
    public int HP
    {
        get { return hp; }
    }

    public bool IsInvincible = false;
    public event Action<G20_Unit> deathActions;
    public event Action<G20_Unit> OnDestroyAction;
    public event Action<G20_Unit> recvDamageActions;
    [SerializeField] G20_ScoreCalculator scoreCaluclator;
    bool isStartDeath = false;

    public void RecvDamage(int damage_value)
    {
        if (0 >= hp || IsInvincible) return;
        damage_value = Mathf.Clamp(damage_value,0,hp);
        scoreCaluclator.CalcAndAddScore(damage_value);
        hp -= damage_value;
        if (recvDamageActions != null) recvDamageActions(this);
        if (0 >= hp)
        {
            ExecuteDeathAction();
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
    public void ExecuteDeathAction()
    {
        if (isStartDeath) return;
        if (deathActions != null) deathActions(this);
        isStartDeath = true;
    }
}
