using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_TitleAssistCounter : MonoBehaviour {
    [SerializeField]float maxAssistValue=60.0f;
    //アシスト
    [SerializeField] int assistCount=10;
    float oneAssistValue;
    // Update is called once per frame
    private void Start()
    {
        oneAssistValue=maxAssistValue / assistCount;
        G20_GameManager.GetInstance().ChangedStateAction += CountShot;
    }
    void Update () {
	}
    //インゲームに入った瞬間これまで撃った弾の弾数-1
    void CountShot(G20_GameState _state)
    {
        switch (_state)
        {
            case G20_GameState.TITLE:
                break;
            case G20_GameState.INGAME:
                Debug.Log(G20_BulletShooter.GetInstance().ShotCount);
                G20_BulletShooter.GetInstance().aimAssistValue=Mathf.Clamp((G20_BulletShooter.GetInstance().ShotCount-1)*oneAssistValue,0,maxAssistValue);
                break;
            case G20_GameState.CLEAR:
                break;
            case G20_GameState.GAMEOVER:
                break;
        }
    }
}
