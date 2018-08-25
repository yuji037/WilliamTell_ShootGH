using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_StageManager : G20_Singleton<G20_StageManager> {

    [SerializeField]
    GameObject stageBehaviourPrefab;
    public void IngameStart()
    {
        G20_Timer.GetInstance().StartTimer(90.0f);
        var stage = Instantiate(stageBehaviourPrefab, transform);
    }
}
