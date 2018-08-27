using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//実行する毎にSEを鳴らして、配列で設定された次のSEに移る。最後まで行ったら最後の音を鳴らし続ける。
public class G20_HitMultiSE : G20_HitAction {
    [SerializeField]G20_SEType[] ses;
    int currentNum=0;
    public override void Execute(Vector3 hit_point)
    {
        G20_SEManager.GetInstance().Play(ses[currentNum], hit_point);
        if (currentNum<ses.Length-1)
        {
            currentNum++;
        }
    }
}
