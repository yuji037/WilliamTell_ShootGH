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
    [SerializeField] float rotspeed=0.3f;
    //移動時間の長さ
    [SerializeField] float runTimeRate = 2.0f;
    //移動時間の最小
    [SerializeField] float runTimeMin = 1.5f;

    [SerializeField] G20_BombController bomb;
    float runTime ;
    float angle = 0.0f;
    bool isRight = true;
    bool rotated = true;


    Vector3 moveVec = new Vector3(0, 0, -1);

    void Start()
    {

        target = GameObject.FindGameObjectWithTag("MainCamera");
        targetPos = target.transform.position;
        targetPos.y = transform.position.y;
        isRight = !(transform.position.x - target.transform.position.x < 0);
        //シューティングの時に使った　名前は知らない
        moveVec = Quaternion.Euler(0, rot / 2 * (isRight ? 1 : -1), 0) * moveVec;
    }

    // Update is called once per frame
    void Update()
    {
        distanceVec = target.transform.position - transform.position;
        distance = distanceVec.magnitude;
        angle = Vector3.Angle(distanceVec, moveVec);
        isRight = (Vector3.Angle(Quaternion.Euler(0, 90, 0) * moveVec, distanceVec) < 90.0f);
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
            //if (distance < attackRange)
            //{
            //    Debug.Log("攻撃開始");
            //    // 攻撃選択
            //}
            //else
            if (distance < changePhase)
            {

                //ある程度近づいたらカメラに向きを合わせる
                yield return StartCoroutine(TargetRun());
                //爆弾なげる
                yield return StartCoroutine(AttackCoroutine());
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
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;
            }

            if (enemy.HP <= 0) yield break;

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

        //なげるアニメーションの実行
        yield return new WaitForSeconds(attacktime);
        
        //アニメーション終了と同時に爆弾の親変更と爆弾の動く処理実行

        if (enemy.HP <= 0) yield break;

        bomb.Bombthrow(attackRange,enemy.Attack);
        bomb.transform.parent = enemy.transform.parent;
        

        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            transform.position += AITime * deathvec;
            yield return null;
            if (transform.position.y < deathposition_y )
            {
                Debug.Log("自殺");
                GetComponent<G20_Unit>().ExecuteDeathAction();
            }
        }
        yield return null;
    }

    IEnumerator RunCoroutine()
    {
        stateController.Run();
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
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            transform.position += moveVec * AITime;


            transform.Rotate(0, bombrot_speed * AITime, 0);


            if (distance <= changePhase)
            {
                // 走ってる途中で近くなったので終了
                yield break;
            }
            yield return null;
        }


        yield return null;
    }

    IEnumerator TargetRun()
    {
        stateController.Run();
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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotspeed);
            yield return null;

        }
        //走る
        //while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        //{

        //    transform.position += targetfront * AITime;
        //    if (distance < attackRange)
        //    {
        //        // 走ってる途中で近くなったので終了
        //        yield break;
        //    }
        //    yield return null;
        //}
        yield return null;
    }

    IEnumerator RotateCoroutine()
    {


        for (float t = 0; t < rotationTime; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }

            moveVec = Quaternion.Euler(0, rot * AITime * (isRight ? 1 : -1), 0) * moveVec;
            transform.Rotate(0, bombrot_speed * AITime, 0);

            transform.position += moveVec * (AITime / 2);

            yield return null;
        }
        yield return null;
    }

}

    


