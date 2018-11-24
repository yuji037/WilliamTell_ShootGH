﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BulletAppleCreator : G20_Singleton<G20_BulletAppleCreator> {
    [SerializeField] GameObject bulletApple;
    [SerializeField] int maxBulletApple;
    int bulletCount;
    public GameObject Create(Vector3 create_point)
    {
        if (maxBulletApple <= bulletCount) return null;
        G20_SEManager.GetInstance().Play(G20_SEType.SUMMON_APPLE,create_point);
        var sa = G20_EffectManager.GetInstance().Create(G20_EffectType.SUMMON_APPLE_VERT, create_point);
        sa.transform.localScale *= 0.5f;
        var ba = Instantiate(bulletApple);
        ba.transform.position = create_point;
        bulletCount++;
        ba.GetComponent<G20_BulletApple>().deathActions +=(a,b)=> bulletCount--;
        return ba;
    }
}
