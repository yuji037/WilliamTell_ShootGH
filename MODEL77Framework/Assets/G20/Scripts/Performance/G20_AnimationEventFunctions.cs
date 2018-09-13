using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AnimationEventFunctions : MonoBehaviour {
    
    public void PlaySE(int num)
    {
        G20_SEManager.GetInstance().Play((G20_SEType)num, Vector3.zero, false);
    }

    public void PlayVoiceWithCaption(int num)
    {
        G20_VoicePerformer.GetInstance().PlayWithCaption((G20_VoiceType)num);
    }
}
