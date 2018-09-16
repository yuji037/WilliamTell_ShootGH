using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDamage : G20_HitAction
{
    [SerializeField]G20_Enemy enemy;
    public G20_Enemy targetEnemy
    {
        get { return enemy; }
    }
    [SerializeField] int damage;
    [SerializeField] G20_DamageType damageType;
    public override void Execute(Vector3 hit_point)
    {
           enemy.RecvDamage(damage, damageType);
    }
}
