using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//hitObjectのactionを起動するclass
public class G20_BulletShooter : G20_Singleton<G20_BulletShooter>
{
    G20_AIMAssistant aIMAssistant = new G20_AIMAssistant();
    //弾の射出を制限出来る
    public bool CanShoot = true;
    //チートモード
    public bool isCheating;
    //AIM補正の値
    public float aimAssistValue = 0f;
    private void Update()
    {

        Vector2? shotPoint = null;

        shotPoint = G20_InputPointGetter.GetInstance().GetInputPoint();
        if (isCheating && shotPoint == null)
        {
            shotPoint = G20_CheatShoot.GetInstance().GetEnemyHeadPoint();
        }
        if (CanShoot && shotPoint != null)
        {
            Vector3 hitPoint = Vector3.zero;
            Vector3 panelhitPoint = Vector3.zero;
            var hitObj = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref hitPoint,G20_HitTag.ASSIST);
            //何にも当たらなかったらAIM補正
            if (aimAssistValue > 0f && !hitObj)
            {
                Vector2 preShot = (Vector2)shotPoint;
                shotPoint = aIMAssistant.AssistAIM((Vector2)shotPoint, aimAssistValue);
                hitObj = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref hitPoint, G20_HitTag.NORMAL | G20_HitTag.ASSIST);
            }

            if (hitObj)
            {
                hitObj.ExcuteActions(hitPoint);
            }

            //effect出す用のパネルのRay判定
            var hitPanel = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref panelhitPoint, G20_HitTag.PANEL);
            if (hitPanel)
            {
                hitPanel.ExcuteActions(panelhitPoint);
            }
        }
    }
}
