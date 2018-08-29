using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Flags]
public enum G20_HitTag{
    NORMAL=0X001,
    //銃撃エフェクトを発生させるパネル
    PANEL=0X002,
    //AIM補正をする対象
    ASSIST=0X004,
}
public class G20_HitObject : MonoBehaviour {
    G20_HitAction[] hitActions;
    public G20_HitTag hitTag=G20_HitTag.NORMAL;
    private void Awake()
    {
        hitActions = GetComponents<G20_HitAction>();    
    }
    public void ExcuteActions(Vector3 hit_point)
    {
        foreach (var i in hitActions)
        {
            i.Execute(hit_point);
        }
    }
}
