using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵リストを保存する箱。
/// </summary>
public class G20_EnemyCabinet : G20_Singleton<G20_EnemyCabinet>
{

    List<G20_Enemy> enemyList = new List<G20_Enemy>();
    public List<G20_Enemy> EnemyList
    {
        get { return enemyList; }
    }
    public int enemyCount { get { return enemyList.Count; }}

    [SerializeField] float unregisterEnemyDelay = 1.0f;

    public void RegisterEnemy(G20_Unit enemy)
    {
        if (!enemy)
        {
            Debug.Log("EnemyCabinetに登録しようとしましたが、対象がnullでした");
            return;
        }

        if (enemy is G20_Enemy)
        {
            enemyList.Add((G20_Enemy)enemy);
        }
        else
        {
            Debug.Log("エラー：敵以外の登録");
        }
    }

    public void UnregisterEnemy(G20_Unit enemy)
    {
        if (!enemy)
        {
            Debug.Log("EnemyCabinetから除外しようとしましたが、対象がnullでした");
            return;
        }

        if (enemy is G20_Enemy)
        {
            enemyList.Remove((G20_Enemy)enemy);
            //enemyCount = enemyList.Count;
            // 見た目上敵が減るのを遅らせる
        }
        else
        {
            Debug.Log("エラー：敵以外の登録解除");
        }
    }

    public void AllEnemyAIStart()
    {
        var enemys = enemyList.ToArray();

        foreach ( var enemy in enemys )
        {
            Debug.Log("AIStart");
            enemy.GetComponent<G20_AI>().AIStart();
        }
    }

    public void DamageAllEnemys(int damage)
    {
        // 敵が死ぬとリストの要素数が変わるためコピー配列で操作する
        var enemys = enemyList.ToArray();

        foreach (var enemy in enemys)
        {
            Debug.Log(enemy + "ダメ―ジ");
            enemy.RecvDamage(damage, G20_DamageType.HEAD);
        }
    }

    public void KillAllEnemys()
    {
        var enemys = enemyList.ToArray();

        foreach (var enemy in enemys)
        {
            if (!enemy) continue;
            Debug.Log(enemy + "キル");
            enemy.RecvDamage(enemy.HP, G20_DamageType.BODY);
        }
    }
}
