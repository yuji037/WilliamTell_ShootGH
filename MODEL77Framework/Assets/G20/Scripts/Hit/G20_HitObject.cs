using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitObject : MonoBehaviour {
    G20_HitAction[] hitActions;

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
