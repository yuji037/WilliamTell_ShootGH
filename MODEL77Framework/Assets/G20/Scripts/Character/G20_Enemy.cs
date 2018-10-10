﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_Enemy : G20_Unit
{
    [SerializeField] int attack = 1;
    //怯みの秒数
    public float hirumiTime = 1.0f;
    //頭にスコア出すためのTransform
    [SerializeField] Transform head;
    public Transform Head { get { return head; } }
    public G20_EnemyAnimation anim;
    public int Attack { get { return attack; } }

    //移動スピード
    [SerializeField, Range(0.5f, 5)] float speed = 1.0f;
    public void SetEnemyAI(G20_AI _ai)
    {
        enemyAI = _ai;
        enemyAI.Init();
    }
    G20_AI enemyAI;
    public G20_AI EnemyAI
    {
        get { return enemyAI; }
    }
    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            speed = Mathf.Clamp(speed, 0.5f, 100);
            anim.AnimSpeed = speed;
        }
    }
    bool isLife = true;
    public bool IsLife { get { return isLife; } }
    //trueの場合怯まない
    public bool isSuperArmor;
    List<G20_EnemyBuff> buffList = new List<G20_EnemyBuff>();
    private void Awake()
    {
        Speed = speed;
        deathActions += (x, y) => KillCollider();
        deathActions += (x, y) => ChangeHitObjectNormal();
        deathActions += (x, y) => isLife = false;
        deathActions += (_, damageType) => UpChainCount(damageType);
        if (!head) head = transform;
    }
    public void AddBuff(G20_EnemyBuff enemy_buff)
    {
        buffList.Add(enemy_buff);
        enemy_buff.StartBuff(() => RemoveBuff(enemy_buff));
    }
    public void RemoveBuff(G20_EnemyBuff enemy_buff)
    {
        enemy_buff.wasRelease = true;
        buffList.Remove(enemy_buff);
    }
    //全部ノーマルに変える
    void ChangeHitObjectNormal()
    {
        var hitObjects = GetComponentsInChildren<G20_HitObject>();
        foreach (var hit in hitObjects)
        {
            hit.ChangeHitTag(G20_HitTag.NORMAL);
        }
    }
    //子オブジェクトのコライダーを非アクティブにする
    void KillCollider()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols)
        {
            c.enabled = false;
        }
    }
}
