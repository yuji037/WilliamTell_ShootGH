using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TestClass {
    int a;
    float b;
}


public class G20_AnimationEventFunctions : MonoBehaviour {
    
    [System.Serializable]
    public class CameraShakeParam {
        public G20_CameraShakeType shakeType;
        public float strength;
        public float frequency;
        public float timeLength;
    }
    [SerializeField]
    CameraShakeParam[] shakeParams;

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform enemy;


    G20_CameraShake cameraShake;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<G20_CameraShake>();
    }

    public void PlaySE(int num)
    {
        G20_SEManager.GetInstance().Play((G20_SEType)num, Vector3.zero, false);
    }

    public void PlayVoiceWithCaption(int num)
    {
        G20_VoicePerformer.GetInstance().PlayWithCaption((G20_VoiceType)num);
    }

    public void CameraShake(int num)
    {
        if ( num >= shakeParams.Length ) return;
        var s = shakeParams[num];
        cameraShake.Shake(s.shakeType, s.strength, s.frequency, s.timeLength);
    }

   
}
