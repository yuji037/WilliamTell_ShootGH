using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class G20_Player :G20_Unit{
    private void Awake()
    {
        //クリア時無敵
        G20_GameManager.GetInstance().ChangedStateAction +=
            _state =>
            {
                if (_state == G20_GameState.CLEAR) IsInvincible = true;
            };
    }
}
