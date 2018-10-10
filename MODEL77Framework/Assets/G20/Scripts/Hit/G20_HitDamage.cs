using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDamage : G20_HitAction
{
    [SerializeField]G20_Unit target;
    public G20_Unit Target
    {
        get { return target; }
    }
    [SerializeField] int damage;
    public override void Execute(Vector3 hit_point)
    {
        target.RecvDamage(damage,G20_Unit.G20_DamageType.Player);
    }
}
