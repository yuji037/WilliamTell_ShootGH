using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_Gessler : G20_Enemy {

    private void Awake()
    {
        deathActions += (a, b) => G20_ScoreManager.GetInstance().Base.AddScore(2000);
    }
}
