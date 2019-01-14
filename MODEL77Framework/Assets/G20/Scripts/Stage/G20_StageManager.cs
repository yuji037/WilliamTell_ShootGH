using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_StageManager : G20_Singleton<G20_StageManager> {

    [SerializeField]
    public GameObject[] stageBehaviourPrefabs;
    public G20_StageBehaviour nowStageBehaviour { get; private set; }
    public void IngameStart()
    {
        var stageType = G20_GameManager.GetInstance().gameDifficulty;
        if ( stageType >= stageBehaviourPrefabs.Length ) stageType = 0;
        var stageObj = Instantiate(stageBehaviourPrefabs[(int)stageType], transform);

        nowStageBehaviour = stageObj.GetComponent<G20_StageBehaviour>();
        float stageTotalTime = nowStageBehaviour.stageTotalTime;
        G20_Timer.GetInstance().StartTimer(stageTotalTime);
    }
}
