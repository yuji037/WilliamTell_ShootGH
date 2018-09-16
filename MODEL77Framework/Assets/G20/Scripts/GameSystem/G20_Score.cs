using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_Score : G20_Singleton<G20_Score>
{
    public event Action<int> ScoreChangedAction;
    public int Score { get; private set; }
    public int GoldPoint { get; private set; }
    void ChangeScore(int _score)
    {
        Score = _score;
        if (ScoreChangedAction != null) ScoreChangedAction(Score);
    }
    public void AddScore(int add_value)
    {
        add_value = Mathf.Abs(add_value);
        ChangeScore( Score + add_value);
    }
    public void SubstractScore(int sub_value)
    {
        sub_value = Mathf.Abs(sub_value);
        ChangeScore(Score - sub_value);
    }
    public void AddGoldPoint(int add_value)
    {
        add_value = Mathf.Abs(add_value);
        GoldPoint += add_value;
    }

}
