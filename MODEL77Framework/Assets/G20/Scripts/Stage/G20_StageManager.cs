using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_StageManager : G20_Singleton<G20_StageManager> {

    [SerializeField]
    public GameObject stageBehaviourPrefab;
    public void IngameStart()
    {
        var stage = Instantiate(stageBehaviourPrefab, transform);

        float stageTotalTime = stage.GetComponent<G20_StageBehaviour>().stageTotalTime;
        G20_Timer.GetInstance().StartTimer(stageTotalTime);
    }
}
