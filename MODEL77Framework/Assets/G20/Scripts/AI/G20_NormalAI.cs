using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_NormalAI : G20_AI
{
    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;
    //移動方向の回転角度
    [SerializeField] float rot = 80;
    //最後のターゲットに向くときの回転スピード
    [SerializeField] float rotspeed = 0.3f;
    //移動時間の長さ制御用
    [SerializeField] float runTimeRate = 2.0f;
    //1回の移動時間の最小
    [SerializeField] float runTimeMin = 1.5f;

    float runTime;
    float angle = 0.0f;
    bool isRight = true;
    bool rotated = true;



    void Start()
    {

        target = GameObject.FindGameObjectWithTag("MainCamera");
        targetPos = target.transform.position;
        targetPos.y = transform.position.y;
        isRight = !(transform.position.x - target.transform.position.x < 0);
        transform.forward = Quaternion.Euler(0, rot / 2 * (isRight ? 1 : -1), 0) * transform.forward;
       
    }


    // Update is called once per frame
    void Update()
    {
        distanceVec = target.transform.position - transform.position;
        distance = distanceVec.magnitude;
        angle = Vector3.Angle(distanceVec, transform.forward);
        isRight = (Vector3.Angle(transform.right, distanceVec) < 90.0f);

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
            if (distance < attackRange )
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
                yield return StartCoroutine(TargetRun());

            }
            else
            {
                if (rotated)
                {
                    yield return StartCoroutine(RunCoroutine());
                    rotated = false;
                }
                else
                {
                    yield return StartCoroutine(RotateCoroutine());
                    rotated = true;
                }
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
        animPlayer.PlayAnimation(G20_AnimType.Attack);

        yield return new WaitForSeconds(attacktime/enemy.Speed);
        if (!enemy.IsLife) yield break;
        G20_EnemyAttack.GetInstance().Attack(enemy.Attack);
    }

    IEnumerator RunCoroutine()
    {
        animPlayer.PlayAnimation(G20_AnimType.Run);

        runTime = (targetPos.x - transform.position.x) * runTimeRate;
        if (runTime < 0)
        {
            runTime *= (-1);
        }
        if (runTime < runTimeMin)
        {
            runTime = runTimeMin;
        }

        for (float t = 0; t < runTime; t += AITime )
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            transform.position += transform.forward * AITime;


            if (distance <= changePhase)
            {
                // 走ってる途中で近くなったので終了
                yield break;
            }
            yield return null;
        }

    }

    IEnumerator TargetRun()
    {
        animPlayer.PlayAnimation(G20_AnimType.Stance);
        enemy.isSuperArmor = true;

        //向き変更
        Vector3 targetfront = distanceVec.normalized;
        targetfront.y = 0;
        for (float t = 0; t < 1.0f; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }

            var newRotation = Quaternion.LookRotation(targetfront).eulerAngles;
            newRotation.x = 0;//ずれ防止のため
            newRotation.z = 0;//ずれ防止のため
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotspeed);
            yield return null;

        }

        yield return null;
        //走る
        animPlayer.PlayAnimation(G20_AnimType.Dash);

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

    IEnumerator RotateCoroutine()
    {

        bool _isRight = isRight;
        for (float t = 0; t < rotationTime; t += AITime )
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            transform.Rotate(0, rot * AITime *(_isRight ? 1 : -1), 0);
            transform.position += transform.forward * (AITime / 2);

            yield return null;
        }

    }

}


