using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_StraightBombAI : G20_AI
{
    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;
    //キャラクターの回転スピード
    [SerializeField] float bombrot_speed = 100;

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
            // 以下の処理（1フレーム間）で行動を決定する

            // ターゲットが目の前にいたら攻撃する
            if (distance < attackRange )
            {
                Debug.Log("攻撃開始");
                // 攻撃選択
                yield return StartCoroutine(AttackCoroutine());
            }
            else
            if (distance < changePhase)
            {
                //ある程度近づいたらカメラに向かう
                yield return StartCoroutine(TargetRun());

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
            yield return null;
        }
    }

    IEnumerator AttackCoroutine()
    {
        if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
        {
            Debug.Log("インゲーム状態を抜けたのでAIを終了");
            yield break;

        }
        Debug.Log("攻撃中");
        stateController.Attack(attacktime, () => G20_EnemyAttack.GetInstance().Attack(enemy.Attack));
        yield return new WaitForSeconds(attacktime);
        
        //攻撃後の消えるまでの処理
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            transform.position += AITime*deathvec;
            yield return null;
            if (transform.position.y < deathposition_y)
            {
                Debug.Log("自殺");
                GetComponent<G20_Unit>().ExecuteDeathAction();
            }
        }
        yield return null;
    }

    //とにかく前に走る
    IEnumerator RunCoroutine()
    {
        //eneAnim.Run();
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

        yield return null;
    }

    //ターゲットに向いてから走る
    IEnumerator TargetRun()
    {
        //eneAnim.Run();

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
        
        //ターゲットに向かって走る
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {

            transform.position += targetfront * AITime;
            if (distance < attackRange )
            {
                // 走ってる途中で近くなったので終了
                yield break;
            }
            yield return null;
        }
        yield return null;
    }
}
