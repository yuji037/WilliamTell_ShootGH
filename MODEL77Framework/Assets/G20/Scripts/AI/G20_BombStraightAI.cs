using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BombStraightAI : G20_AI
{
    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;
    //キャラクターの回転スピード
    [SerializeField] float bombrot_speed = 100;

    [SerializeField] G20_BombController bomb;
    Vector3 moveVec = new Vector3(0, 0, -1);

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera");
        targetPos = target.transform.position;
        targetPos.y = transform.position.y;
    }


    // Update is called once per frame
    void Update()
    {
        distanceVec = target.transform.position - transform.position;
        distance = distanceVec.magnitude;

    }

    protected override void childAIStart()
    {
        StartCoroutine(AICoroutine());
    }



    IEnumerator AICoroutine()
    {

        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            while (isPouse)
            {
                yield return null;
            }
            // 以下の処理（1フレーム間）で行動を決定する

            if (distance < changePhase)
            {
                //ある程度近づいたらカメラに向かう
                yield return StartCoroutine(TargetRun());

                yield return StartCoroutine(AttackCoroutine());

                yield return StartCoroutine(SusideCoroutine());

            }
            else
            {
                yield return StartCoroutine(RunCoroutine());
            }

            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            if (!enemy.IsLife) yield break;
        }
    }

    IEnumerator AttackCoroutine()
    {
        enemy.isSuperArmor = true;
        if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
        {
            Debug.Log("インゲーム状態を抜けたのでAIを終了");
            yield break;

        }
        Debug.Log("攻撃中");
        animPlayer.PlayAnimation(G20_AnimType.Attack);
        //なげるアニメーションの実行

        yield return new WaitForSeconds(attacktime / enemy.Speed);
        //アニメーション終了と同時に爆弾の親変更と爆弾の動く処理実行

        if (!enemy.IsLife) yield break;

        bomb.Bombthrow(attackRange, enemy.Attack);
        bomb.transform.parent = enemy.transform.parent;
        
       
    }

    //とにかく前に走る
    IEnumerator RunCoroutine()
    {
        animPlayer.PlayAnimation(G20_AnimType.Run);

        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            transform.Rotate(0, bombrot_speed * AITime, 0);

            transform.position += moveVec * AITime;
            if (distance < changePhase)
            {
                yield break;
            }
            yield return null;
        }

      
    }

    //ターゲットに向いてから走る
    IEnumerator TargetRun()
    {
        animPlayer.PlayAnimation(G20_AnimType.Run);

        //ターゲットに向く
        Vector3 targetfront = distanceVec.normalized;
        for (float t = 0; t < rotationTime; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }

            var newRotation = Quaternion.LookRotation(targetfront).eulerAngles;
            newRotation.x = 0;　//ずれ防止のため
            newRotation.z = 0;　//ずれ防止のため
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), 0.3f);
            yield return null;

        }
        targetfront.y = 0;
        
        ////ターゲットに向かって走る
        //while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        //{

        //    transform.position += targetfront * AITime;
        //    if (distance < attackRange )
        //    {
        //        // 走ってる途中で近くなったので終了
        //        yield break;
        //    }
        //    yield return null;
        //}

    }
}
