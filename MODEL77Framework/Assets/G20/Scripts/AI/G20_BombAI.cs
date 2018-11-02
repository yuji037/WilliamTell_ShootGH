using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アニメーションはアニメーションクロスフェード

public class G20_BombAI : G20_AI
{
    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;
    //移動方向の回転角度
    [SerializeField] float rot = 80;
    //キャラクターの回転スピード
    [SerializeField] float bombrot_speed = 100;
    //移動方向の回転スピード
    [SerializeField] float rotspeed = 0.3f;
    //移動時間の長さ
    [SerializeField] float runTimeRate = 2.0f;
    //移動時間の最小
    [SerializeField] float runTimeMin = 1.5f;

    [SerializeField] G20_BombController bomb;
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
        //シューティングの時に使った　名前は知らない
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
            if (distance < changePhase)
            {

                //ある程度近づいたらカメラに向きを合わせる
                yield return StartCoroutine(TargetRun());
                //爆弾なげる
                yield return StartCoroutine(AttackCoroutine());
                //自殺
                yield return StartCoroutine(SusideCoroutine());
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
        enemy.isSuperArmor = true;
        if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
        {
            //Debug.Log("インゲーム状態を抜けたのでAIを終了");
            yield break;
        }
        //Debug.Log("攻撃中");
        animPlayer.PlayAnimation(G20_AnimType.Attack);

        //なげるアニメーションの実行
        yield return new WaitForSeconds(attacktime / enemy.Speed);
        
        //アニメーション終了と同時に爆弾の親変更と爆弾の動く処理実行

        if (!enemy.IsLife) yield break;

        bomb.Bombthrow(attackRange,enemy.Attack);
        bomb.transform.parent = enemy.transform.parent;
        bomb = null;
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

        for (float t = 0; t < runTime; t += AITime)
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
        animPlayer.PlayAnimation(G20_AnimType.Run);

        //向き変更
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
            newRotation.x = 0;//ずれ防止のため
            newRotation.z = 0;//ずれ防止のため
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotspeed);
            yield return null;

        }
        
    }

    IEnumerator RotateCoroutine()
    {


        bool _isRight = isRight;

        for (float t = 0; t < rotationTime; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }

            transform.Rotate(0, rot * AITime * (_isRight ? 1 : -1), 0);
            transform.position += transform.forward * (AITime / 2);
            
            yield return null;
        }
    }

}

    


