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
            enemy.EnemyAI.AIStart();
        }
    }

    public void DamageAllEnemys(int damage)
    {
        // 敵が死ぬとリストの要素数が変わるためコピー配列で操作する
        var enemys = enemyList.ToArray();

        StartCoroutine(DamageEffectPositions(enemys,damage)); ;
    }

    IEnumerator DamageEffectPositions(G20_Enemy[] enemys, int damage)
    {
        float interval = 0.07f;

        foreach (var enemy in enemys )
        {
            if ( !enemy ) continue;

            DamageEffectSEEnemy(enemy);
            enemy.RecvDamage(damage, G20_DamageType.HEAD);
            yield return new WaitForSeconds(interval);
        }
    }

    public void DamageEffectSEEnemy(G20_Enemy enemy)
    {
        G20_EffectManager effMn = G20_EffectManager.GetInstance();
        G20_SEManager seMn = G20_SEManager.GetInstance();
        Vector3 pos = enemy.Head.position;

        G20_EffectManager.GetInstance().Create(G20_EffectType.HIT_APPLE_HEAD, pos);
        G20_SEManager.GetInstance().Play(G20_SEType.HIT_HEAD, pos);
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
