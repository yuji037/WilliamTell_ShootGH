using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//game開始時からのtimeを記録するclass
public class G20_Timer : G20_Singleton<G20_Timer> {
    public float FirstTime { get; private set; }
    public float CurrentTime { get; private set; }
    public event  Action TimerZeroAction;
    bool wasStart=false;
    public void StartTimer(float take_time)
    {
        if (wasStart) return;
        CurrentTime = take_time;
        FirstTime = take_time;
        StartCoroutine(CountTimer());
        wasStart = true;
    }
    private IEnumerator CountTimer()
    {
        while (CurrentTime >= 0)
        {
            CurrentTime -= Time.deltaTime;
            yield return null;
        }
        if (TimerZeroAction!=null)
        {
            TimerZeroAction();
        }
    }
}
