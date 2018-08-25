using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDamage : G20_HitAction
{
    [SerializeField]G20_Enemy enemy;
    [SerializeField] int damage;
    public override void Execute(Vector3 hit_point)
    {
           enemy.RecvDamage(damage);
    }
}
