using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDeActive : G20_HitAction
{
    [SerializeField] GameObject deActiveObject;
    public override void Execute(Vector3 hit_point)
    {
        deActiveObject.SetActive(false);
    }
}
