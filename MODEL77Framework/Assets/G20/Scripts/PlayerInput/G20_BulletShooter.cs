using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//hitObjectのactionを起動するclass
public class G20_BulletShooter : G20_Singleton<G20_BulletShooter>
{
    G20_AIMAssistant aIMAssistant = new G20_AIMAssistant();
    [SerializeField] Camera effectCamera;
    //弾の射出を制限出来る
    public bool CanShoot = true;
    //チートモード
    public bool isCheating;
    //AIM補正の値
    public float aimAssistValue = 0f;
    int shotCount;
    public int ShotCount
    {
        get { return shotCount; }
    }
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

            shotCount++;

            Vector3 hitPoint = Vector3.zero;
            //AIM補正
            if (aimAssistValue > 0)
            {
                shotPoint = aIMAssistant.AssistAIM((Vector2)shotPoint, aimAssistValue);
            }
            else
            {
                Debug.Log("補正しません");
            }
            var hitObj = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref hitPoint,Camera.main);
            if (hitObj)
            {
                hitObj.ExcuteActions(hitPoint);
            }

            //effect出す用のパネルのRay判定
            Vector3 panelhitPoint = Vector3.zero;
            var hitPanel = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref panelhitPoint,effectCamera);
            if (hitPanel)
            {
                hitPanel.ExcuteActions(panelhitPoint);
            }
        }
    }
}
