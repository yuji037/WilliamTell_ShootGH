using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//hitObjectのactionを起動するclass
public class G20_BulletShooter : G20_Singleton<G20_BulletShooter>
{
    [SerializeField] LayerMask hitmask;
    [SerializeField] LayerMask panelmask;
    //弾の射出を制限出来る
    public bool CanShoot=true;
    //チートモード
    public bool isCheating;
    private void Update()
    {
   
        Vector2? shotPoint = null;
       
        shotPoint= G20_InputPointGetter.GetInstance().GetInputPoint();
        if (isCheating&&shotPoint==null)
        {
            shotPoint = G20_CheatShoot.GetInstance().GetEnemyHeadPoint();
        }
        if (CanShoot&&shotPoint != null)
        {
            Vector3 hitPoint = Vector3.zero;
            Vector3 panelhitPoint = Vector3.zero;
            var hitObj = G20_RayShooter.GetHit<G20_HitObject>((Vector2)shotPoint, ref hitPoint, hitmask);
            var hitPanel = G20_RayShooter.GetHit<G20_HitObject>((Vector2)shotPoint, ref panelhitPoint, panelmask);
            if (hitObj)
            {
                hitObj.ExcuteActions(hitPoint);
            }
            if (hitPanel)
            {
                hitPanel.ExcuteActions(panelhitPoint);
            }
        }
    }
}
