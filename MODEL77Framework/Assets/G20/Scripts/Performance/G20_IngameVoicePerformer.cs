using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_IngameVoicePerformer : G20_IngamePerformer
{
    [SerializeField] G20_VoiceType voiceType;
    public override void StartPerformance()
    {
        Debug.Log("voicePlay");
        G20_VoicePerformer.GetInstance().Play(voiceType);
    }
}
