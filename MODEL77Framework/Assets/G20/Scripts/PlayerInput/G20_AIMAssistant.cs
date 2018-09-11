using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIMAssistant
{
    public Vector2 AssistAIM(Vector2 shot_point, float assist_value)
    {
        float shortestDistance = 3000000f;
        Vector2 nearestDiff = Vector2.zero;
        foreach (var e in G20_HitObjectCabinet.GetInstance().AssitObjectList)
        {
            ChangeNearest(ref nearestDiff,ref shortestDistance, shot_point, e.transform.position);
        }
        //AIM補正のポイントがターゲットを通り過ぎないようにする
        if (assist_value > shortestDistance) assist_value = shortestDistance;
        return shot_point + (nearestDiff.normalized * assist_value);
    }
    void ChangeNearest(ref Vector2 nearest_diff,ref float shortest_distance, Vector2 shot_point, Vector3 target_postion)
    {
        Vector2 targetPoint = Camera.main.WorldToScreenPoint(target_postion);
        var diffVec = targetPoint - shot_point;
        var dis = diffVec.magnitude;
        //最短距離じゃなければnullを返す
        if (shortest_distance > dis) {
            shortest_distance = dis;
            nearest_diff= diffVec;
        };
    }
    public void AimasistvalueSet()
    {

        float assistlate = 1 - ((float)(G20_Score.GetInstance().Score + 1) / (float)G20_BulletShooter.GetInstance().ShotCount);
        if (assistlate < 0) assistlate = 0;

        G20_BulletShooter.GetInstance().aimAssistValue = G20_BulletShooter.GetInstance().aimAssistValueMax * assistlate;
    }
}
