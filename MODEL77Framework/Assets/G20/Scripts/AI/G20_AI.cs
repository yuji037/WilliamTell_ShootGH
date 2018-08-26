using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class G20_AI : MonoBehaviour
{
    public bool isPouse;
    public G20_Enemy enemy;
    protected GameObject target;
    protected Vector3 targetPos;
    protected Vector3 distanceVec = Vector3.zero;
    protected float distance = 9999;
    protected G20_StateController stateController;
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


    private void Awake()
    {
        enemy = GetComponent<G20_Enemy>();
        stateController = new G20_StateController(this);
        stateController.Start();
        StartCoroutine(StateRoutine());
    }
    IEnumerator StateRoutine()
    {
        while (true)
        {
            stateController.Update();
            yield return null;
        }
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
        if (enemy.HP <= 0) return;
        childAIStart();
    }
    protected abstract void childAIStart();

}
