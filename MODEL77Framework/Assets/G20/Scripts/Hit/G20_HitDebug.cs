using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitDebug : G20_HitAction {

    public static bool IsPressedButton = false;

    //private void Awake()
    //{
    //    //base.Awake();
    //    IsPressedButton = false;
    //}

    private void Update()
    {
        // 毎フレームリセット
        IsPressedButton = false;
    }

    public override void Execute(Vector3 hit_point)
    {
        Debug.Log("デバッグボタン押された");
        IsPressedButton = true;
    }
}

public sealed class WaitForDebugButtonInput : CustomYieldInstruction {

    private float timeoutTime;

    public override bool keepWaiting
    {
        get
        {
            if ( G20_HitDebug.IsPressedButton )
            {
                return false;
            }
            if(Time.realtimeSinceStartup >= timeoutTime )
            {
                return false;
            }

            return true;
        }
    }

    public WaitForDebugButtonInput(float waitTime)
    {
        this.timeoutTime = Time.realtimeSinceStartup + waitTime;
    }
    
}
