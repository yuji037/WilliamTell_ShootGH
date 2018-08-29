using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class G20_RayShooter
{
    //HitObject用RayCast、Rayを飛ばしてカメラから一番近いHitTagのHitObjectインスタンスを返す
    public static G20_HitObject GetHitObject(Vector2 screen_pos, ref Vector3 hit_point, G20_HitTag hit_tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(screen_pos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000.0f);
        float shortest = 100000000f;
        G20_HitObject retObj = null;
        foreach (var hit in hits)
        {
            var hitObj = hit.transform.GetComponent<G20_HitObject>();
            if (!hitObj) continue;
            bool isMatchTag = (hitObj.hitTag & hit_tag)>0;
            if (!(isMatchTag))
            {
                continue;
            }
            var sqrMag = Vector3.SqrMagnitude(hit.point - Camera.main.transform.position);
            if (shortest > sqrMag)
            {
                shortest = sqrMag;
                hit_point = hit.point;
                retObj = hitObj;
            }
        }
        return retObj;
    }
}
