using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDebugMessage : G20_HitAction
{
    [SerializeField]string debugMessage;
    public override void Execute(Vector3 hit_point)
    {
        Debug.Log(debugMessage);
    }
}
