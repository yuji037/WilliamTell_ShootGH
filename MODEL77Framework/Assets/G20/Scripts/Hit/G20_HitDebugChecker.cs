using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDebugChecker : G20_HitAction {
    [SerializeField] GameObject checkObj;
    public override void Execute(Vector3 hit_point)
    {
        checkObj.SetActive(!checkObj.activeSelf);
    }
}
