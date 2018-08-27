using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug用勝手頭を撃つ
public class G20_CheatShoot : G20_Singleton<G20_CheatShoot> {
   
	public Vector2? GetEnemyHeadPoint()
    {
        foreach(var i in G20_EnemyCabinet.GetInstance().EnemyList)
        {
            if (i.transform.position.y>=0)
            {
                return Camera.main.WorldToScreenPoint(i.Head.transform.position);
            }
        }
        return null;
    }
}
