using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_EnemyPopper : MonoBehaviour {

    [SerializeField]
    GameObject[] enemyPrefabs;

    [SerializeField]
    Transform enemysParent;

    [SerializeField]
    // 下から出てくる
    float spawnHeight = -2.0f;
    [SerializeField]
    float riseUpSpeed = 3.0f;

    public Transform PopCenter;

    public void EnemyPop(G20_EnemyType enemyType, Vector3 position,G20_EnemyBuff enemy_buff=null)
    {
        // 敵のオブジェクト生成
        var ene = Instantiate(enemyPrefabs[(int)enemyType],enemysParent);
        ene.transform.position = position + new Vector3(0, spawnHeight, 0);

        //敵の召喚エフェクト生成(少し魔法陣を浮かす)
        G20_EffectManager.GetInstance().Create(G20_EffectType.SUMMON_APPLE, position + new Vector3(0,0.02f,0));
        G20_SEManager.GetInstance().Play(G20_SEType.SUMMON_APPLE, position +new Vector3(0, 0.02f, 0));
        // 出現演出
        StartCoroutine(RiseUp(ene));

        var enemy = ene.GetComponent<G20_Enemy>();
        
        //enemyのbuffを設定、本来はstageBehaviourで処理したいので後で移行
        if (G20_Timer.GetInstance().CurrentTime<=30.0f)
        {
            enemy_buff = new G20_SpeedBuff(enemy,100.0f,1.2f);
        }
        if (enemy_buff!=null)
        {
            enemy.AddBuff(enemy_buff);
        }
        // 敵情報保存
        G20_EnemyCabinet.GetInstance().RegisterEnemy(enemy);

        //敵情報破棄を敵が死んだときに実行
        enemy.deathActions += G20_EnemyCabinet.GetInstance().UnregisterEnemy;
    }

    // 下から出てくる演出
    IEnumerator RiseUp(GameObject ene)
    {
        //float[] rotPatern = { -45.0f, -22.5f, 22.5f, 45.0f };
        //ene.transform.Rotate(0, rotPatern[Random.Range(0, rotPatern.Length)], 0);
        //魔法陣が出来てから少し間を置く
        yield return new WaitForSeconds(0.3f);

        var enemy = ene.GetComponent<G20_Enemy>();

        while ( ene && enemy.HP > 0 && ene.transform.position.y < 0 )
        {
            ene.transform.position += new Vector3(0, riseUpSpeed * Time.deltaTime, 0);
            yield return null;
        }
        if (!ene || enemy.HP <= 0) yield break;
        // Y座標を0に補正
        Vector3 pos = ene.transform.position;
        ene.transform.position = new Vector3(pos.x, 0, pos.z);

        // 敵AI開始
        ene.GetComponent<G20_AI>().AIStart();
    }
}
