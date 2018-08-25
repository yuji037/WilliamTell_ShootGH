using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class G20_HitAction : MonoBehaviour {
    public abstract void Execute(Vector3 hit_point);
}
