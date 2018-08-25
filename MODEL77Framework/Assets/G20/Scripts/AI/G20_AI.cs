using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class G20_AI : MonoBehaviour
{
    [SerializeField]protected G20_EnemyAnimation eneAnim;
    bool isPouse;
    G20_Enemy ene;
    Coroutine currentRoutine;
    protected bool isAttacking;
    int? currentState;

    protected GameObject target;
    protected Vector3 targetPos;
    protected Vector3 distanceVec = Vector3.zero;
    protected float distance = 9999;

    //攻撃に移行する距離
    [SerializeField] protected float attackRange = 3.0f;
    //キャラクターを消す高さ
    [SerializeField] protected float deathposition_y = 2.0f;
    //自殺の時の沈むスピードと向き
    [SerializeField] protected Vector3 deathvec = new Vector3(0, -2, 0);
    //攻撃している時間
    [SerializeField] protected float attacktime = 1.0f;
    //行動パターンの変更距離
    [SerializeField] protected float changePhase = 5.0f;


    private void  Awake()
    {
        ene = GetComponent<G20_Enemy>();
        ene.recvDamageActions += PouseAI;
        ene.deathActions += _=>DeathAI();
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
                return Time.deltaTime*ene.Speed;
            }
        }
    }
    //引数の秒数に合わせてplayerにダメージ
    protected void Attack(float damage_delay_time)
    {
        eneAnim.Attack();
        currentRoutine = StartCoroutine(AttackCoroutine(damage_delay_time));
    }
    IEnumerator AttackCoroutine(float damage_delay_time)
    {
        isAttacking = true;
        yield return new WaitForSeconds(damage_delay_time);
        //isAttackingが中断された時は、damage処理をしない
        if (isAttacking)
        {
            G20_EnemyAttack.GetInstance().Attack(ene.Attack);
        }
        isAttacking = false;
    }
    void DeathAI()
    {
        if ( !this ) return;
        if(currentRoutine!=null)StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine( DeathCoroutine(1.0f));
    }
    IEnumerator DeathCoroutine(float _time)
    {
        eneAnim.Death();
        isPouse = true;
        isAttacking = false;
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
    //ここに書くべきじゃないと感じるので後で他の場所に移す
    int maxhirumi = 3;
    int hirumi = 0;
    //attackモーション中はAiをStopしない
    void PouseAI(G20_Unit _unit)
    {
        if (maxhirumi<=hirumi||isAttacking||_unit.HP<=0) return;
        hirumi++;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(PouseRoutine(1.0f));
    }
    IEnumerator PouseRoutine(float _time)
    {
        if(currentState==null)currentState=eneAnim.GetCurrentState();
        eneAnim.Falter();
        isPouse = true;
        yield return new WaitForSeconds(_time);
        isPouse = false;
        eneAnim.SetState((int)currentState);
        currentState = null;
    }
    public void AIStart()
    {
        if (ene.HP <= 0) return;
        childAIStart();
    }
    protected abstract void childAIStart();

}
