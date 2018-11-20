using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class G20_AI : MonoBehaviour
{
    bool isAIStarted = false;
    public bool IsAIStarted
    {
        get { return isAIStarted; }
    }
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
        enemy.deathActions += (x, y) => DeathEnemy();
        ChildInit();
    }
    protected virtual void ChildInit()
    {
    }
    void DeathEnemy()
    {
        isPouse = true;
        if (enemy.HP > 0) return;
        if (animPlayer) animPlayer.PlayAnimation(enemy.deathAnimTypes, 1.0f, () => Destroy(gameObject));
        if (animPlayer) animPlayer.isEnding = true;
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


    protected IEnumerator SusideCoroutine()
    {
        while (true)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                //Debug.Log("インゲーム状態を抜けたのでAIを終了");
                yield break;
            }

            transform.position += Time.deltaTime * deathvec;

            if (transform.position.y < deathposition_y)
            {
                if (!enemy.IsLife) yield break;

                //Debug.Log("自殺");
                GetComponent<G20_Unit>().ExecuteDeathAction(G20_Unit.G20_DamageType.Enemy);
                Destroy(gameObject);

            }

            yield return null;
        }
    }
}
