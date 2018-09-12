using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum G20_ReceiveType
{
    Normal,
    Golden,
}
[System.Serializable]
public class G20_DamageReceiver
{
    [SerializeField] G20_ReceiveType receiveType;
    //頭に当たった時のダメージ倍率
    [SerializeField] uint HeadRate = 1;
    public int ReceiveDamage(int _damage, G20_DamageType damageType, G20_Enemy owner)
    {
        switch (receiveType)
        {
            case G20_ReceiveType.Normal:
                switch (damageType)
                {
                    case G20_DamageType.HEAD:
                        _damage *= (int)HeadRate;
                        G20_EffectManager.GetInstance().Create(G20_EffectType.PLUS_ONE_SCORE, owner.Head.position);
                        G20_Score.GetInstance().AddScore(1);
                        break;
                    case G20_DamageType.BODY:
                        break;
                }
                break;
            case G20_ReceiveType.Golden:
                switch (damageType)
                {
                    case G20_DamageType.HEAD:
                        _damage *= (int)HeadRate;
                        _damage=Mathf.Clamp(_damage, 0, owner.HP);
                        int score = 0;
                        score = _damage / (int)HeadRate;
                        var obj = G20_EffectManager.GetInstance().Create(G20_EffectType.PLUS_ONE_SCORE, owner.Head.position);
                        obj.GetComponent<TextMesh>().text = "+" + score;
                        G20_Score.GetInstance().AddScore(score);
                        break;
                    case G20_DamageType.BODY:
                        break;
                }
                break;
        }
        return _damage;
    }
}
