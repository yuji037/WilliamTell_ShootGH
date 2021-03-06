﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public struct G20_Score
{
    public event Action<int> ScoreChangedAction;
    public int Value { get; private set; }
    void ChangeScore(int _value)
    {
        Value = _value;
        if (ScoreChangedAction != null) ScoreChangedAction(Value);
    }
    public void AddScore(int add_value)
    {
        add_value = Mathf.Abs(add_value);
        ChangeScore(Value + add_value);
    }
    public void SubstractScore(int sub_value)
    {
        sub_value = Mathf.Abs(sub_value);
        ChangeScore(Value - sub_value);
    }
}
public class G20_ScoreManager : G20_Singleton<G20_ScoreManager>
{
    public G20_Score Base;
    public G20_Score GoldPoint;
    public G20_ScoreCalcData scoreCalcData;
    public int GetBaseScore()
    {
        return Base.Value;
    }
    //BaseとBonusの合計スコア
    public int GetSumScore()
    {
        return Base.Value + GetHitRateBonus() + GetMaxChainBonus();
    }
    public int GetHitRateBonus()
    {
        return scoreCalcData.HitRateMultyply * (int)(G20_BulletShooter.GetInstance().HitRate * 100);
    }
    public int GetMaxChainBonus()
    {
        return scoreCalcData.MaxChainMultiply * G20_ChainCounter.GetInstance().MaxChainCount;
    }
}
