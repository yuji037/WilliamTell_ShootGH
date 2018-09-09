using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class G20_RayShooter
{
    //HitObject用RayCast、Rayを飛ばしてカメラから一番近いHitTagのHitObjectインスタンスを返す
    public static G20_HitObject GetHitObject(Vector2 screen_pos, ref Vector3 hit_point, Camera ray_camera,LayerMask layer_mask)
    {
        Ray ray = ray_camera.ScreenPointToRay(screen_pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f,layer_mask)){
            var hitObj = hit.transform.GetComponent<G20_HitObject>();
            if (!hitObj) return null;
            hit_point = hit.point;
            return hitObj;

        }
        return null;
    }
}
