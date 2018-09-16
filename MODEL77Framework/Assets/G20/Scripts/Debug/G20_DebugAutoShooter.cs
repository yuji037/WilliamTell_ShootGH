using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
class ShooterParam
{
    //見やすくするため名称を付ける
    [SerializeField] string paramName;
    //基本のインターバル
    public float baseShotInterval;  
    //追加のランダムインターバル
    public float addRandShotInterval;
    //基本のブレ
    public float baseOffsetRadius;
    //追加でランダムにブレる範囲
    public float addRandOffsetRadius;
    //enemyCountがこの値以上の場合Enemyを優先して攻撃する
    public int priorityEnemyCount;
}
public class G20_DebugAutoShooter : MonoBehaviour
{
    [SerializeField] ShooterParam[] param;
    //-1はランダム
    [SerializeField] int paramNumber = -1;
    [SerializeField] bool isAutoShooting;
    [SerializeField] G20_HitObject[] titleApples;
    float timer = 0f;
    private void Awake()
    {
        IsAutoShooting = isAutoShooting;
    }
    //自動射撃切り替え
    public bool IsAutoShooting
    {
        get
        {
            return isAutoShooting;
        }
        set
        {
            if (value)
            {
                if (paramNumber < 0 || paramNumber >= param.Length) paramNumber = UnityEngine.Random.Range(0, param.Length);
                G20_BulletShooter.GetInstance().ChangeGetShotPointFunc(GetShotPoint);
                SetTimer();
            }
            else
            {
                G20_BulletShooter.GetInstance().RemoveGetShotPointFunc();
            }
            isAutoShooting = value;
        }
    }
    public Vector2? GetShotPoint()
    {
        if (!CanShoot()) return null;
        //タイトル時にタイトルアップルを狙う
        if (G20_GameManager.GetInstance().gameState == G20_GameState.TITLE)
        {
            var num=UnityEngine.Random.Range(0, titleApples.Length);
            return Camera.main.WorldToScreenPoint(titleApples[num].transform.position);
        }
        var shotPoint = SearchShotPoint();
        if (shotPoint != null)
        {
            var radius = param[paramNumber].baseOffsetRadius;
            var randDir = GetRandomDir();
            shotPoint += radius*randDir;
            var randRad = param[paramNumber].addRandOffsetRadius;
            shotPoint += randDir*UnityEngine.Random.Range(0, randRad);
            return shotPoint;
        }
        return null;
    }
    bool CanShoot()
    {
        if (timer <= 0f)
        {
            SetTimer();
            return true;
        }
        else
        {
            return false;
        }
    }
    void SetTimer()
    {
        timer = param[paramNumber].baseShotInterval + UnityEngine.Random.Range(0, param[paramNumber].addRandShotInterval);
    }
    Vector2 GetRandomDir()
    {
        //全角度からランダムで取得
        var radian = UnityEngine.Random.Range(0, (float)Math.PI * 2.0f);
        var randVec = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        return randVec.normalized;
    }
    Vector2? SearchShotPoint()
    {
        foreach (var i in G20_HitObjectCabinet.GetInstance().AssitObjectList)
        {
            if (!i.GetComponent<Collider>().enabled) continue;
            if (IsEnemy(i.gameObject))
            {
                if (CanShootEnemy(i.gameObject))
                {
                    var pos=Camera.main.WorldToScreenPoint(i.transform.position);
                    if (InScreenRange(pos))
                    {
                        return pos;
                    }
                }
            }
            else if (G20_EnemyCabinet.GetInstance().enemyCount < param[paramNumber].priorityEnemyCount)
            {
                var pos = Camera.main.WorldToScreenPoint(i.transform.position);
                if (InScreenRange(pos))
                {
                    return pos;
                }
            }
        }
        return null;
    }
    bool InScreenRange(Vector2 pos)
    {
        bool inWidth = 0 <= pos.x && pos.x <= Screen.width;
        bool inHeight = 0 <= pos.y && pos.y <= Screen.height;
        return (inWidth && inHeight);
    }
    bool CanShootEnemy(GameObject _enemy)
    {
        var ai = _enemy.GetComponent<G20_HitDamage>().targetEnemy.EnemyAI;
        if (ai && ai.IsAIStarted)
        {
            return true;
        }
        return false;
    }
    bool IsEnemy(GameObject _gameObject)
    {
        return _gameObject.GetComponent<G20_HitDamage>();
    }
    
    private void Update()
    {
        timer -= Time.deltaTime;
    }
}
