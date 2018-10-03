using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_SmallStraightAI : G20_AI
{
    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;
    
    //ジャンプしたときの放物線のするどさ
    [SerializeField] float gravity = 0.9f;
    //大きければ大きいほど上までとぶ
    [SerializeField] float init_v = 1.2f;
    //最後のターゲットに向くときの回転スピード
    [SerializeField] float rotspeed = 0.3f;

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

            // ターゲットが目の前にいたら攻撃する
            if (distance < attackRange)
            {
                //Debug.Log("攻撃開始");
                // 攻撃選択
                yield return StartCoroutine(AttackCoroutine());

                yield return StartCoroutine(SusideCoroutine());

            }
            else
            if (distance < changePhase)
            {
                //ある程度近づいたらカメラに向かう
                yield return StartCoroutine(TargetJump());

            }
            else
            {
                yield return StartCoroutine(RunCoroutine());
            }

            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            if (!enemy.IsLife) yield break;
        }
    }

    IEnumerator AttackCoroutine()
    {
        if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
        {
            //Debug.Log("インゲーム状態を抜けたのでAIを終了");
            yield break;

        }

        //Debug.Log("攻撃中");
        G20_EnemyAttack.GetInstance().Attack(enemy.Attack);
        // Debug.Log("攻撃終了");

    }

    IEnumerator TargetJump()
    {
        //体の向き回転
        Vector3 targetfront = distanceVec.normalized;
        targetfront.y = 0;
        for (float t = 0; t < rotationTime; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            var newRotation = Quaternion.LookRotation(targetfront).eulerAngles;
            newRotation.x = 0; //ずれ防止のため
            newRotation.z = 0; //ずれ防止のため
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotspeed);
            yield return null;

        }

        yield return null;
        if (!enemy.IsLife) yield break;
        //ジャンプモーション開始
        animPlayer.PlayAnimation(G20_AnimType.Jump);
        yield return new WaitForSeconds(1.0f / enemy.anim.AnimSpeed);
        //放物線
        float hight = 0;
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME && transform.position.y >= -0.1f)
        {

            transform.position += (!enemy.IsLife ? -1 : 1) * transform.forward * Time.deltaTime * enemy.Speed;

            if (!enemy.IsLife && init_v > 0) init_v = 0;
            init_v -= gravity * Time.deltaTime * (!enemy.IsLife ? 4 : 1);
            hight += init_v * Time.deltaTime;


            transform.position = new Vector3(transform.position.x, hight, transform.position.z);

            if (distance < attackRange)
            {
                // 走ってる途中で近くなったので終了
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator RunCoroutine()
    {
        animPlayer.PlayAnimation(G20_AnimType.Run);

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

    


}
