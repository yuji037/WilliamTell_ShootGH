using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerにAttackするだけ
public class G20_EnemyAttack :G20_Singleton<G20_EnemyAttack>{
    [SerializeField] G20_Player player;
    public void Attack(int attack_value)
    {
        player.RecvDamage(attack_value);
    }
}
