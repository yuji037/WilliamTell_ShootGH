using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_Enemy : G20_Unit {
    [SerializeField] int attack = 1;
    public G20_EnemyAnimation anim;
    public int Attack { get { return attack; } }
    public float Speed = 1.0f;
    List<G20_EnemyBuff> buffList=new List<G20_EnemyBuff>();
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
}
