using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class G20_AIState{
    public float endTime;
    float timer = 0f;
    protected G20_AIState(float end_time,G20_AI _owner) { endTime = end_time; owner= _owner; }
    protected abstract G20_AIState Update();
    protected G20_AI owner;
    public  G20_AIState BaseUpdate()
    {
        timer += Time.deltaTime;
        return Update();
    }
    public abstract void OnStart();
    public abstract  void OnEnd();
    protected bool CheckOver() {
        return (endTime<=timer);
    }
}
