using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug用勝手頭を撃つ
public class G20_CheatShoot : G20_Singleton<G20_CheatShoot> {
   
	public Vector2? GetEnemyHeadPoint()
    {
        foreach(var i in G20_HitObjectCabinet.GetInstance().AssitObjectList)
        {
            if (i.GetComponent<Collider>().enabled&&i.transform.position.y>=0)
            {
                return Camera.main.WorldToScreenPoint(i.transform.position);
            }
        }
        return null;
    }
}
