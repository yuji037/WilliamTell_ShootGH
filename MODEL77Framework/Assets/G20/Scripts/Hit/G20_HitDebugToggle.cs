using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_HitDebugToggle: G20_HitAction {
    [SerializeField] GameObject checkObj;
    public event Action<bool> toggleAction;
    bool isActive;
    public override void Execute(Vector3 hit_point)
    {
        isActive = !isActive;
        Debug.Log(isActive);
        checkObj.SetActive(isActive);
        if (toggleAction != null) toggleAction(isActive);
    }
}
