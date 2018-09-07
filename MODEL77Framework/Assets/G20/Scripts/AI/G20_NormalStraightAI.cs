﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_NormalStraightAI : G20_AI {

    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;

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

                yield return StartCoroutine(SusideCoroutine());

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
            if (enemy.HP <= 0) yield break;
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
        stateController.Attack(attacktime, ()=>G20_EnemyAttack.GetInstance().Attack(enemy.Attack));
        yield return new WaitForSeconds(attacktime);
       
    }

    IEnumerator RunCoroutine()
    {
        stateController.Run();
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            transform.position += transform.forward * AITime;
            if (distance < changePhase)
            {
                yield break;
            }
            yield return null;
        }

    }

    IEnumerator TargetRun()
    {
        stateController.Stance();
        //向き変更
        Vector3 targetfront = distanceVec.normalized;
        targetfront.y = 0;
        for (float t = 0; t < rotationTime; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }

            var newRotation = Quaternion.LookRotation(targetfront).eulerAngles;
            newRotation.x = 0;//ずれ防止のため
            newRotation.z = 0;//ずれ防止のため
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), 0.3f);
            yield return null;

        }

        yield return null;
        //走る
        stateController.Dash();
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            transform.position += transform.forward * AITime;
            if (distance < attackRange )
            {
                // 走ってる途中で近くなったので終了
                yield break;
            }
            yield return null;
        }
    }

   

   
}
