using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitBomb : G20_HitAction {

    [SerializeField] int damage = 9;

    public override void Execute(Vector3 hit_point)
    {
        G20_EnemyCabinet.GetInstance().DamageAllEnemys(damage);
        gameObject.SetActive(false);
    }
}