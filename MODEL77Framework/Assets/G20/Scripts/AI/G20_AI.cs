using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class G20_AI : MonoBehaviour
{
    bool isAIStarted = false;
    public bool isPouse;
    public G20_Enemy enemy;
    protected GameObject target;
    protected Vector3 targetPos;
    protected Vector3 distanceVec = Vector3.zero;
    protected float distance = 9999;
    protected G20_EnemyAnimation animPlayer;
    //攻撃に移行する距離
    [SerializeField] protected float attackRange = 3.0f;
    //キャラクターを消す高さ
    [SerializeField] protected float deathposition_y = -2.0f;
    //自殺の時の沈むスピードと向き
    [SerializeField] protected Vector3 deathvec = new Vector3(0, -2, 0);
    //攻撃している時間
    [SerializeField] protected float attacktime = 1.0f;
    //行動パターンの変更距離
    [SerializeField] protected float changePhase = 5.0f;



    public void Init()
    {
        enemy = GetComponent<G20_Enemy>();
        animPlayer = enemy.anim;
        //0だったら怯まないように設定
        if (enemy.hirumiTime > 0f)
        {
            enemy.recvDamageActions += _ => Falter();
        }
        enemy.deathActions += _ => animPlayer.PlayAnimation(G20_AnimType.Death);
        //死んだときにデストロイするように設定
        enemy.deathActions += _ => StartCoroutine(DestroyCoroutine(1.0f / (enemy.anim.AnimSpeed)));
    }

    protected float AITime
    {
        get
        {
            if (isPouse)
            {
                return 0f;
            }
            else
            {
                return Time.deltaTime * enemy.Speed;
            }
        }
    }

    public void AIStart()
    {
        if (isAIStarted || !enemy.IsLife) return;
        isAIStarted = true;
        childAIStart();
    }

    protected abstract void childAIStart();

    G20_AnimType lastAnim;
    float falterTime;

    void Falter()
    {
        if (enemy.isSuperArmor || !enemy.IsLife) return;
        StartCoroutine(FalterRoutine());
    }
    IEnumerator FalterRoutine()
    {
        if (!isPouse) lastAnim = animPlayer.lastAnim;
        animPlayer.PlayAnimation(G20_AnimType.Falter, enemy.hirumiTime);
        falterTime = enemy.hirumiTime;
        if (isPouse)yield break;
        isPouse = true;

        while (falterTime > 0)
        {
            yield return null;
            falterTime -= Time.deltaTime;
        }
        if (enemy.IsLife)
        {
            //最後のTime
            animPlayer.PlayAnimation(lastAnim);
            isPouse = false;
        }
    }
    protected IEnumerator DestroyCoroutine(float duration_time)
    {
        isPouse = true;
        yield return new WaitForSeconds(duration_time);
        Destroy(gameObject);
    }
    protected IEnumerator SusideCoroutine()
    {
        while (true)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;
            }

            transform.position += Time.deltaTime * deathvec;

            if (transform.position.y < deathposition_y)
            {
                if (!enemy.IsLife) yield break;

                Debug.Log("自殺");
                GetComponent<G20_Unit>().ExecuteDeathAction();
                Destroy(gameObject);

            }

            yield return null;
        }
    }
}
