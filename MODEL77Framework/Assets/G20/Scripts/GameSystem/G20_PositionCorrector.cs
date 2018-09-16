using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class G20_PositionCorrector
{
    public static Vector3 Correct(Vector3 pos, float correct_value=0.5f)
    {
        var correctVec = (Camera.main.transform.position - pos).normalized * correct_value;
        return pos + correctVec;
    }
}
