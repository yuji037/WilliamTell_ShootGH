using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AIMAssistant {
    public Vector2  AssistAIM( Vector2 shot_point,float assist_value) {
        float min = 3000f;
        Vector2 mindiff=Vector2.zero;
        foreach (var e in G20_EnemyCabinet.GetInstance().EnemyList)
        {
            Vector2 enemyPoint= Camera.main.WorldToScreenPoint(e.Head.transform.position);
            var diff = enemyPoint-shot_point;
            var mag= diff.magnitude;
            if (min > mag)
            {
                min = mag;
                mindiff = diff;
            }
        }
        if (min >= 1000f) return shot_point;
        if (assist_value > min) assist_value=min;
        Debug.Log(mindiff.normalized * assist_value);
        return shot_point+(mindiff.normalized * assist_value);
    }
}
