using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class G20_RayShooter
{
    //スクリーン上の引数の位置からrayを飛ばして指定のclassを返す。
    public static T GetHit<T>(Vector2 screen_pos,ref Vector3 hit_point,LayerMask mask)
        where T : MonoBehaviour
    {

        //GUIにHitするとnullを返す。
        if (IsUGUIHit(screen_pos))
        {
            return null;
        }
        Ray ray = Camera.main.ScreenPointToRay(screen_pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            T retObj = hit.transform.GetComponent<T>();
            if (retObj)
            {
                hit_point = hit.point;
                return retObj;
            }
        }

        return null;
    }
    //GUIにhitしてたらtrueを返す。
    public static bool IsUGUIHit(Vector3 screen_pos)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current)
        {
            position = screen_pos
        };
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        return (result.Count > 0);
    }
}
