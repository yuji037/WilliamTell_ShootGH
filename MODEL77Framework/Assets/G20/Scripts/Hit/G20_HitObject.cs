using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum G20_HitTag
{
    NORMAL = 0X001,
    //銃撃エフェクトを発生させるパネル
    PANEL = 0X002,
    //AIM補正をする対象
    ASSIST = 0X004,
}
public class G20_HitObject : MonoBehaviour {
    G20_HitAction[] hitActions;
    [SerializeField]G20_HitTag hitTag=G20_HitTag.NORMAL;
    [SerializeField] bool isHitRateUP;
    public bool IsHitRateUp
    {
        get { return isHitRateUP; }
    }
    public G20_HitTag HitTag
    {
        get { return hitTag; }
    }
    private void Awake()
    {
        hitActions = GetComponents<G20_HitAction>();
        G20_HitObjectCabinet.GetInstance().Add(this);
    }
    private void OnDestroy()
    {
        G20_HitObjectCabinet.GetInstance().Remove(this);
    }
    public void ChangeHitTag(G20_HitTag hit_tag)
    {
        hitTag = hit_tag;
        G20_HitObjectCabinet.GetInstance().UpdateTagList(this);
    }
    public void ExcuteActions(Vector3 hit_point)
    {
        foreach (var i in hitActions)
        {
            if ( i != null ) i.Execute(hit_point);
        }
    }
}
