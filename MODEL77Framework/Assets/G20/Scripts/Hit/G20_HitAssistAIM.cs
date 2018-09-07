using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitAssistAIM : G20_HitAction {
    //最大AIM補正値
    //[SerializeField] float maxAssistValue = 60.0f;
    //最大AIM補正値になるまでの回数
    //[SerializeField] int assistCount = 10;
    //Update is called once per frame
    public override void Execute(Vector3 hit_point)
    {
    //    var oneAssistValue = maxAssistValue / assistCount;
    //    //0以上AIM最大補正値以下になるようにClamp関数で制限
    //    G20_BulletShooter.GetInstance().aimAssistValue = Mathf.Clamp((G20_BulletShooter.GetInstance().ShotCount - 1) * oneAssistValue, 0, maxAssistValue);
    }
}
