using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定数やenumをここで定義する
// 自由に追加・編集してOK

public enum G20_GameState {
    TITLE,
    INGAME,
    CLEAR,
    GAMEOVER,
}

public enum G20_EnemyType {
    NORMAL = 0,
    GOLDEN,
    BOMB,
    SMALL,
    NORMAL_STRAIGHT,
    GOLDEN_STRAIGHT,
    BOMB_STRAIGHT,
    SMALL_STRAIGHT,
    SMALL_BULLET,
    //EnemyTypeMax,
}