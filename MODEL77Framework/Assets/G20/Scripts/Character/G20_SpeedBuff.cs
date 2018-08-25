using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_SpeedBuff : G20_EnemyBuff
{
    float plusSpeed;
    public G20_SpeedBuff(G20_Enemy _enemy, float duration_time, float plus_speed) : base(_enemy, duration_time)
    {
        plusSpeed = plus_speed;
    }

    protected override void ApplyBuff()
    {
        enemy.Speed +=plusSpeed;
    }

    protected override void ReleaseBuff()
    {
        enemy.Speed -= plusSpeed;
    }
}
