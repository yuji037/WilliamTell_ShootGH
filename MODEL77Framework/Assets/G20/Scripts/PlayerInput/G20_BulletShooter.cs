using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//hitObjectのactionを起動するclass
public class G20_BulletShooter : G20_Singleton<G20_BulletShooter>
{
    [SerializeField] LayerMask hitmask;
    [SerializeField] LayerMask panelmask;
    public bool CanShoot=true;
    private void Update()
    {
        Vector2? shotPoint = G20_InputPointGetter.GetInstance().GetInputPoint();
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
