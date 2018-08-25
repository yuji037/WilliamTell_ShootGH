using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitToIngame : G20_HitAction {

    public override void Execute(Vector3 hit_point)
    {
        G20_GameManager.GetInstance().StartIngameCoroutine();
    }

}
