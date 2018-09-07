using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitToIngame : G20_HitAction {

    [SerializeField]
    G20_StageType stageType;

    public override void Execute(Vector3 hit_point)
    {
        G20_StageManager.GetInstance().stageType = stageType;

        G20_GameManager.GetInstance().StartIngameCoroutine();
    }

}
