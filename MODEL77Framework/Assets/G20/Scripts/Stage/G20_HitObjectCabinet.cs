using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitObjectCabinet : G20_Singleton<G20_HitObjectCabinet> {
    List<G20_HitObject> hitObjectList=new List<G20_HitObject>();
    List<G20_HitObject> assitObjectList=new List<G20_HitObject>();
    public List<G20_HitObject> AssitObjectList
    {
        get { return assitObjectList; }
    }
    public void UpdateTagList(G20_HitObject hit_object)
    {
        if (assitObjectList.Contains(hit_object)&&hit_object.HitTag!=G20_HitTag.ASSIST)
        {
            assitObjectList.Remove(hit_object);
        }
        if (!assitObjectList.Contains(hit_object) && hit_object.HitTag==G20_HitTag.ASSIST)
        {
            assitObjectList.Add(hit_object);
        }
    }
    public void Add(G20_HitObject hit_object)
    {
        hitObjectList.Add(hit_object);
        if (hit_object.HitTag == G20_HitTag.ASSIST)
        {
            assitObjectList.Add(hit_object);
        }
    }
    public void Remove(G20_HitObject hit_object)
    {
        hitObjectList.Remove(hit_object);
        if (hit_object.HitTag == G20_HitTag.ASSIST)
        {
            assitObjectList.Remove(hit_object);
        }
    }
}
