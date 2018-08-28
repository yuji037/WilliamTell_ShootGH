using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BossAI : G20_AI
{
    //ボス本体の変数
    [SerializeField] float movespeed = 1;
    
    //ふわふわリンゴの変数
    [SerializeField] float rotspeed = 100;
    [SerializeField] GameObject apple;

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
        StartCoroutine(AppleCoroutine());
    }

    IEnumerator AICoroutine()
    {
       
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            if (distance < attackRange)
            {
                yield return StartCoroutine(Attack_Coroutine());
            }
            else
            {
                yield return StartCoroutine(Move_Coroutine());
            }
            yield return null;
        }
    }

    IEnumerator Move_Coroutine()
    {
        Vector3 moveVec = new Vector3(0, 0, (-1) * movespeed);
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            if (distance < attackRange)
            {
                yield break;
            }

            transform.position += moveVec * AITime;
            yield return null;
        }
        yield return null;
    }

    IEnumerator Attack_Coroutine()
    {
        if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
        {
            Debug.Log("インゲーム状態を抜けたのでAIを終了");
            yield break;

        }
        Debug.Log("攻撃中");

        stateController.Attack(attacktime, () => G20_EnemyAttack.GetInstance().Attack(enemy.Attack));

        yield return new WaitForSeconds(attacktime);

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

    IEnumerator AppleCoroutine()
    {
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME)
        {
            apple.transform.Rotate(0, rotspeed * AITime, 0);
            if (enemy.HP <= 0)
            {
                Destroy(apple);
                yield break;
            }
            yield return null;
        }
    }
}
