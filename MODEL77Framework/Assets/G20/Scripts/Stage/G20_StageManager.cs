using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum G20_StageType {
    NORMAL,
    HARD,
}

public class G20_StageManager : G20_Singleton<G20_StageManager> {

    public G20_StageType stageType;

    [SerializeField]
    public GameObject stageBehaviourPrefab;
    public G20_StageBehaviour nowStageBehaviour { get; private set; }
    public void IngameStart()
    {
        var stageObj = Instantiate(stageBehaviourPrefab, transform);

        nowStageBehaviour = stageObj.GetComponent<G20_StageBehaviour>();
        float stageTotalTime = nowStageBehaviour.stageTotalTime;
        G20_Timer.GetInstance().StartTimer(stageTotalTime);
    }
}
