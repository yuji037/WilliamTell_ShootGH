using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class G20_RayShooter
{
    //Rayを飛ばして当たったインスタンスを返す
    public static T GetHitObject<T>(Vector2 screen_pos, ref Vector3 hit_point, Camera ray_camera,LayerMask layer_mask,float range=1000.0f)
        where T:MonoBehaviour
    {
        Ray ray = ray_camera.ScreenPointToRay(screen_pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range,layer_mask)){
            var hitObj = hit.transform.GetComponent<T>();
            if (!hitObj) return null;
            hit_point = hit.point;
            return hitObj;
        }
        return null;
    }
}
