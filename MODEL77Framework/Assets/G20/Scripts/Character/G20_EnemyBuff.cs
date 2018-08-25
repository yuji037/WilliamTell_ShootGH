using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class G20_EnemyBuff
{
    protected G20_Enemy enemy;
    float durationTime;
    public bool wasRelease=false;
    public G20_EnemyBuff(G20_Enemy _enemy, float duration_time)
    {
        enemy = _enemy;
        durationTime=duration_time;
    }
    public void StartBuff(Action on_end_action)
    {
        enemy.StartCoroutine(buffCoroutine(durationTime, on_end_action));
    }
    protected abstract void ApplyBuff();
    IEnumerator buffCoroutine(float duration_time,Action on_end_action)
    {
        ApplyBuff();
        yield return new WaitForSeconds(duration_time);
        if (!wasRelease) ReleaseBuff();
        on_end_action();
    }
    protected abstract void ReleaseBuff();
}
