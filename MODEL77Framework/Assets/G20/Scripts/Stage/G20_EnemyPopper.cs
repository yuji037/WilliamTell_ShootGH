using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum G20_EnemyPopType {
    RISE_UP,
    WALK_FORWARD,
    FLYING_FORWARD,
}

public class G20_EnemyPopper : MonoBehaviour
{
    [SerializeField]
    G20_PopEnemySelector popEnemySelector;
    [SerializeField]
    Transform enemysParent;

    [SerializeField]
    // 下から出てくる
    float riseUpDifY = -2.0f;
    [SerializeField]
    float riseUpSpeed = 3.0f;

    [SerializeField]
    // 奥から歩いてくる
    float walkForwardDifZ = 1.0f;
    [SerializeField]
    float walkForwardSpeed = 3.0f;

    // 一律全敵に付けるバフ
    public float onPopSpeedBuffValue = 0f;

    public G20_Enemy EnemyPop(G20_EnemyType enemyType, Vector3 position, G20_EnemyPopType popType = G20_EnemyPopType.RISE_UP)
    {
        // 敵のオブジェクト生成
        var ene = popEnemySelector.GetPopEnemy(enemyType);
        ene.transform.SetParent(transform.parent);
        
        G20_SEManager.GetInstance().Play(G20_SEType.SUMMON_APPLE, position + new Vector3(0, 0.02f, 0));
        // 出現演出
        switch ( popType )
        {
            case G20_EnemyPopType.RISE_UP:
                //敵の召喚エフェクト生成(少し魔法陣を浮かす)
                G20_EffectManager.GetInstance().Create(G20_EffectType.SUMMON_APPLE, position + new Vector3(0, 0.02f, 0));
                ene.transform.position = position + new Vector3(0, riseUpDifY, 0);
                StartCoroutine(RiseUp(ene));
                break;
            case G20_EnemyPopType.WALK_FORWARD:
                G20_EffectManager.GetInstance().Create(G20_EffectType.SUMMON_APPLE_VERT, position);
                position.y = 0f;
                ene.transform.position = position + new Vector3(0, 0, walkForwardDifZ);
                StartCoroutine(WalkForward(ene));
                break;
        }

        var enemy = ene.GetComponent<G20_Enemy>();

        //enemyのbuffを設定
        G20_EnemyBuff enemy_buff = null;
  
        if (onPopSpeedBuffValue != 0) enemy_buff = new G20_SpeedBuff(enemy, 100.0f, onPopSpeedBuffValue);

        if (enemy_buff != null)
        {
            enemy.AddBuff(enemy_buff);
        }
        // 敵情報保存
        G20_EnemyCabinet.GetInstance().RegisterEnemy(enemy);

        //敵情報破棄を敵が死んだときに実行
        enemy.OnDestroyAction += G20_EnemyCabinet.GetInstance().UnregisterEnemy;

        return enemy;
    }
    
    // 一時的に敵を非表示
    IEnumerator InvisibleDuringTime(GameObject ene, float time)
    {
        List<Renderer> rens = new List<Renderer>(ene.GetComponentsInChildren<Renderer>());
        foreach(var r in rens )
        {
            r.enabled = false;
        }

        yield return new WaitForSeconds(time);

        foreach ( var r in rens )
        {
            r.enabled = true;
        }
    }

    // 奥から地面の高さを出てくる演出
    IEnumerator WalkForward(GameObject ene)
    {
        var enemy = ene.GetComponent<G20_Enemy>();
        //float[] rotPatern = { -45.0f, -22.5f, 22.5f, 45.0f };
        //ene.transform.Rotate(0, rotPatern[Random.Range(0, rotPatern.Length)], 0);
        StartCoroutine(InvisibleDuringTime(ene, 0.3f));
        //魔法陣が出来てから少し間を置く
        yield return new WaitForSeconds(0.3f);


        while ( ene && enemy.HP > 0 && ene.transform.position.y < 0 )
        {
            ene.transform.position = new Vector3(0, 0, -walkForwardSpeed * Time.deltaTime);
            yield return null;
        }
        if ( !ene || !enemy.IsLife ) yield break;

        // 敵AI開始
        if ( G20_GameManager.GetInstance().gameState == G20_GameState.INGAME )
        {
            var eneAI = ene.GetComponent<G20_Enemy>().EnemyAI;
            if(eneAI)eneAI.AIStart();
        }

    }

    // 下から出てくる演出
    IEnumerator RiseUp(GameObject ene)
    {
        var enemy = ene.GetComponent<G20_Enemy>();
        //float[] rotPatern = { -45.0f, -22.5f, 22.5f, 45.0f };
        //ene.transform.Rotate(0, rotPatern[Random.Range(0, rotPatern.Length)], 0);
        //魔法陣が出来てから少し間を置く
        yield return new WaitForSeconds(0.3f);


        while (ene && enemy.HP > 0 && ene.transform.position.y < 0)
        {
            ene.transform.position += new Vector3(0, riseUpSpeed * Time.deltaTime, 0);
            yield return null;
        }
        if (!ene || !enemy.IsLife) yield break;
        // Y座標を0に補正
        Vector3 pos = ene.transform.position;
        ene.transform.position = new Vector3(pos.x, 0, pos.z);

        // 敵AI開始
        if (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            var eneAI = ene.GetComponent<G20_Enemy>().EnemyAI;
            if(eneAI)eneAI.AIStart();
        }

    }
}
