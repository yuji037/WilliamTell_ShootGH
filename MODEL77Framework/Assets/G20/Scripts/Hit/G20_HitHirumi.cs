using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitHirumi : G20_HitAction {
    [SerializeField] G20_Enemy enemy;
    [SerializeField] int hirumiCount;
    public override void Execute(Vector3 hit_point)
    {
        if (hirumiCount > 0)
        {
            enemy.Falter();
            hirumiCount--;
        }
    }
}
