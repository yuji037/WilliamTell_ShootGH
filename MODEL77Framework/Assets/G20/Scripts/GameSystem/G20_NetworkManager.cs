using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class G20_NetworkManager : MonoBehaviour
{
    [SerializeField] string adress = "http://";
    [SerializeField] string ip = "127.0.0.1:10080";
    [SerializeField] string dir = "/gp17op17/WT/";
    string userIDstr;
    public int userID;
    int difficulty;

    string scoreSendAdress;
    string scoreReceiveAdress;
    string IDReceiveAdress;

    bool scoreSendComp = false;
    //オフラインモードでの実行はfalse
    [SerializeField] bool networkflag = false;

    int month;
    int day;

    // Use this for initialization
    void Start()
    {
        scoreReceiveAdress = adress + ip + dir + "ScoreSend.php";
        scoreSendAdress = adress + ip + dir + "ScoreReceive.php";
        IDReceiveAdress = adress + ip + dir + "IDSend.php";
        Scorereceive();
        IDReceive();
        month = DateTime.Now.Month;
        day = DateTime.Now.Day;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME) scoreSendComp = false;

        if (G20_GameManager.GetInstance().gameState == G20_GameState.CLEAR)
        {
            Scoresend();
        }
        Debug.Log("send:" + scoreSendComp);
    }

    ////////////////↓受信系↓////////////////////////////
    //ランキング用スコアの取得
    public void Scorereceive()
    {
        if (networkflag) StartCoroutine(ScoreReceiveCoroutine());
    }
    //現状取得してくるだけ
    IEnumerator ScoreReceiveCoroutine()
    {
        WWW www = new WWW(scoreReceiveAdress);
        yield return www;

        Debug.Log("スコア情報(CSV)：" + www.text);

        yield return null;
    }

    //プレイヤーのID番号取得
    public void IDReceive()
    {
        if (networkflag) StartCoroutine(IDReceiveCoroutine());
    }
    IEnumerator IDReceiveCoroutine()
    {
        WWW www = new WWW(IDReceiveAdress);
        yield return www;
        userIDstr = www.text;
        Debug.Log("ユーザーID : " + userIDstr);
        yield return null;
    }

    ////////////////↑受信系↑////////////////////////////


    ////////////////↓送信系↓////////////////////////////
    public void Scoresend()
    {
        if (networkflag && !scoreSendComp)
        {
            StartCoroutine(ScoreSendCoroutine());
            scoreSendComp = true;
        }
    }
    IEnumerator ScoreSendCoroutine()
    {

        WWWForm form = new WWWForm();
        form.AddField("userinfo", "Guest");
        form.AddField("month", month);
        form.AddField("day", day);
        form.AddField("date", 0904);
        form.AddField("score", G20_Score.GetInstance().Score);
        form.AddField("ID", userIDstr);
        form.AddField("difficulty", 1);

        WWW www = new WWW(scoreSendAdress, form);
        yield return www;

        Debug.Log(www.text);

        yield return null;
    }
    ////////////////↑送信系↑////////////////////////////

}
