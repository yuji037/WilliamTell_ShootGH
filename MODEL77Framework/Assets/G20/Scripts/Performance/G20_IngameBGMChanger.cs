using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_IngameBGMChanger : G20_IngamePerformer {

    [SerializeField]
    G20_BGMType playBGMType;

    public override void StartPerformance()
    {
        G20_BGMManager.GetInstance().Play(playBGMType);
    }
}
