using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIMAssistant
{
    public Vector2 AssistAIM(Vector2 shot_point, float assist_value)
    {
        float shortestDistance = 3000000f;
        Vector2 nearestDiff = Vector2.zero;
        foreach (var e in G20_EnemyCabinet.GetInstance().EnemyList)
        {
            ChangeNearest(ref nearestDiff,ref shortestDistance, shot_point, e.Head.transform.position);
            if (!CheckEnemyMotimono(e)) continue;
            ChangeNearest(ref nearestDiff,ref shortestDistance, shot_point, e.motimono.transform.position);
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
    //motimonoのtagがassistの場合true
    bool CheckEnemyMotimono(G20_Enemy _enemy)
    {
        if (!_enemy.motimono) return false;
        var hitObj = _enemy.motimono.GetComponent<G20_HitObject>();
        if (!hitObj) return false;
        if (!(hitObj.hitTag == G20_HitTag.ASSIST)) return false;
        return true;
    }
}
