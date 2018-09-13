using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AnimationEventFunctions : MonoBehaviour {
    
    public void PlaySE(int num)
    {
        G20_SEManager.GetInstance().Play(G20_SEType.TEST_VOICE, Vector3.zero, false);
    }

    public void PlayVoice(int num)
    {
        G20_VoicePerformer.GetInstance().PlayWithSubtitle((G20_VoiceType)num);
    }
}
