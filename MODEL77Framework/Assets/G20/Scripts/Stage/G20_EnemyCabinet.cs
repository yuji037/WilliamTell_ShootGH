using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵リストを保存する箱。
/// </summary>
public class G20_EnemyCabinet : G20_Singleton<G20_EnemyCabinet> {

    List<G20_Enemy> enemyList = new List<G20_Enemy>();
    public int enemyCount { get; private set; }

    private void Start()
    {
        enemyCount = 0;
    }

    public void RegisterEnemy(G20_Unit enemy)
    {
        if ( !enemy )
        {
            Debug.Log("EnemyCabinetに登録しようとしましたが、対象がnullでした");
            return;
        }

        if (enemy is G20_Enemy )
        {
            enemyList.Add((G20_Enemy)enemy);
            enemyCount = enemyList.Count;
        }
        else
        {
            Debug.Log("エラー：敵以外の登録");
        }
    }

    public void UnregisterEnemy(G20_Unit enemy)
    {
        if ( !enemy )
        {
            Debug.Log("EnemyCabinetから除外しようとしましたが、対象がnullでした");
            return;
        }

        if ( enemy is G20_Enemy )
        {
            enemyList.Remove((G20_Enemy)enemy);
            enemyCount = enemyList.Count;
        }
        else
        {
            Debug.Log("エラー：敵以外の登録解除");
        }
    }

    public void DamageAllEnemys(int damage)
    {
        // 敵が死ぬとリストの要素数が変わるためコピー配列で操作する
        var enemys = enemyList.ToArray();

        foreach(var enemy in enemys )
        {
            Debug.Log(enemy + "ダメ―ジ");
            enemy.RecvDamage(damage);
        }
    }

    public void KillAllEnemys()
    {
        var enemys = enemyList.ToArray();

        foreach ( var enemy in enemys )
        {
            if ( !enemy ) continue;
            Debug.Log(enemy + "キル");
            enemy.RecvDamage(enemy.HP);
        }
    }
}
