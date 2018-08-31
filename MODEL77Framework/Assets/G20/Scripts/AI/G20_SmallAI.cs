using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_SmallAI : G20_AI
{

    //移動方向の回転にかける時間
    [SerializeField] float rotationTime = 1.0f;
    //移動方向の回転にかける時間
    [SerializeField] float rot = 80f;
    //ジャンプしたときの放物線のするどさ
    [SerializeField] float gravity = 0.9f;
    //大きければ大きいほど上までとぶ
    [SerializeField] float init_v = 1.2f;
    //最後のターゲットに向くときの回転スピード
    [SerializeField] float rotspeed=0.3f;

    float runTime;
    float angle = 0.0f;
    bool isRight = true;
    bool rotated = true;

    [SerializeField] float runTimeRate = 2.0f;
    [SerializeField] float runTimeMin = 1.5f;

    // Use this for initialization
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
        distanceVec.y = 0;
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
            // 以下の処理（1フレーム間）で行動を決定する

            // ターゲットが目の前にいたら攻撃する
            if (distance < attackRange )
            {
                    // 攻撃選択
                    yield return StartCoroutine(AttackCoroutine());
            }
            else
            if (distance < changePhase)
            {
                //ある程度近づいたらカメラに向かう
                yield return StartCoroutine(TargetJump());

            }
            else
            {
                //向き変更とまっすぐ進むを交互に
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


        //自殺
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            transform.position += AITime * deathvec;
            yield return null;
            if (transform.position.y < deathposition_y)
            {
                Debug.Log("自殺");
                GetComponent<G20_Unit>().ExecuteDeathAction();
            }
        }
        yield return null;
    }

    IEnumerator RunCoroutine()
    {
        //走る時間の計算
        runTime = (targetPos.x - transform.position.x) * runTimeRate;
        if (runTime < 0) runTime *= (-1);
        if (runTime < runTimeMin) runTime = runTimeMin;

        //走る
        for (float t = 0; t < runTime; t += AITime)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
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


        yield return null;
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
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;

            }
            var newRotation = Quaternion.LookRotation(targetfront).eulerAngles;
            newRotation.x = 0; //ずれ防止のため
            newRotation.z = 0; //ずれ防止のため
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotspeed);
            yield return null;

        }

        yield return null;
        //ジャンプモーション開始
        stateController.Attack(100.0f, null);
        yield return new WaitForSeconds(1.0f/enemy.anim.AnimSpeed);
        //放物線
        float hight = 0;
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {

            transform.position += transform.forward * AITime;
            init_v -= gravity * AITime;
            hight += init_v * AITime;
            transform.position = new Vector3(transform.position.x, hight, transform.position.z);

            if (distance < attackRange )
            {
                // 走ってる途中で近くなったので終了
                yield break;
            }
            yield return null;
        }
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
            transform.Rotate(0, rot * AITime * (isRight ? 1 : -1), 0);
            transform.position += transform.forward * (AITime / 2);

            yield return null;
        }
        yield return null;
    }
    
}
