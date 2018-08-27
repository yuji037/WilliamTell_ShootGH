using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_Enemy : G20_Unit {
    [SerializeField] int attack = 1;
    //怯みの秒数
    public float hirumiTime=1.0f;
    //頭にスコア出すためのTransform
    [SerializeField] Transform head;
    //頭に当たった時のダメージ倍率
    [SerializeField] uint HeadRate=1;
    public G20_EnemyAnimation anim;
    public int Attack { get { return attack; } }
    //移動スピード
    [SerializeField] float speed=1.0f;
    public float Speed
    {
        get { return speed; }
        set {
            speed = value;
            speed=Mathf.Clamp(speed,0,100);
        }
    }
    List<G20_EnemyBuff> buffList=new List<G20_EnemyBuff>();
    private void Awake()
    {
        Speed = speed;
        anim.AnimSpeed = Speed;
        deathActions +=_=> KillCollider();
        if (!head) head = transform;
    }
    public void AddBuff(G20_EnemyBuff enemy_buff)
    {
        buffList.Add(enemy_buff);
        enemy_buff.StartBuff(()=>RemoveBuff(enemy_buff));
    }
    public void RemoveBuff(G20_EnemyBuff enemy_buff)
    {
        enemy_buff.wasRelease = true;
        buffList.Remove(enemy_buff);
    }
    protected override void uRecvDamage(int damage_value, G20_DamageType damage_type)
    {
        
        switch (damage_type)
        {
            case G20_DamageType.HEAD:
                damage_value *= (int)HeadRate;
                G20_EffectManager.GetInstance().Create(G20_EffectType.PLUS_ONE_SCORE, head.position);
                G20_Score.GetInstance().AddScore(1);
                break;
            case G20_DamageType.BODY:
                break;
        }
        hp -= damage_value;
    }
    //子オブジェクトのコライダーを非アクティブにする
    void KillCollider()
    {
        var cols=GetComponentsInChildren<Collider>();
        foreach (var c in cols)
        {
            c.enabled = false;
        }
    }
}
