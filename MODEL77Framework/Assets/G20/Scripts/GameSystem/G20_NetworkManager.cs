using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class G20_NetworkManager : G20_Singleton<G20_NetworkManager>
{
    public G20_SQLModel userData;

    string userIDstr;
    public int userID;

    [SerializeField] string adress  = "http://";
    [SerializeField] string ip      = "192.168.40.129";
    [SerializeField] string dir     = "/gp17op17/WT/";
    [SerializeField] string scoreSendFile = "ScoreReceive.php";
    [SerializeField] string scoreReceiveFile = "ScoreSend.php";
    [SerializeField] string IDReceiveFile = "IDSend.php";


     string scoreSendAdress   ;
     string scoreReceiveAdress;
     string IDReceiveAdress   ;




    bool scoreSendComp = false;
    //オフラインモードでの実行はfalse
    [SerializeField] public bool networkflag = false;
    
    int date;

    // Use this for initialization
    void Start()
    {
        scoreSendAdress = adress + ip + dir + scoreSendFile;
        Debug.Log("スコア送信アドレス : " + scoreSendAdress);
        scoreReceiveAdress = adress + ip + dir + scoreReceiveFile;
        Debug.Log("スコア受信アドレス : " + scoreReceiveAdress);
        IDReceiveAdress = adress + ip + dir + IDReceiveFile;
        Debug.Log("ID受信アドレス : " + IDReceiveAdress);
        date = DateTime.Now.Month * 100 + DateTime.Now.Day;

        IDReceive();
        G20_GameManager.GetInstance().ChangedStateAction += Scoresend;
        G20_GameManager.GetInstance().ChangedStateAction += Scorereceive;

        //Scoresendtest();
    }

    // Update is called once per frame
    void Update()
    {
        if (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME) scoreSendComp = false;

       
        Debug.Log("send:" + scoreSendComp);
    }

    ////////////////↓受信系↓////////////////////////////
    //ランキング用スコアの取得
    public void Scorereceive(G20_GameState state)
    {

        if (networkflag && state == G20_GameState.INGAME)
        {
            StartCoroutine(ScoreReceiveCoroutine());
        }
    }
    //現状取得してくるだけ
    IEnumerator ScoreReceiveCoroutine()
    {
        Debug.Log("スコア表受信開始 難易度：" + G20_StageManager.GetInstance().stageType + (int)G20_StageManager.GetInstance().stageType);
        WWWForm form = new WWWForm();
        form.AddField("date", date);
        form.AddField("difficulty", (int)G20_StageManager.GetInstance().stageType);

        WWW www = new WWW(scoreReceiveAdress, form);
        yield return www;
        Debug.Log("スコア表：" + www.text);
      
        string jsonText = "{ \"scoreList\" : " + www.text + "}";


        Debug.Log("jsonText" + jsonText);

        userData = JsonUtility.FromJson<G20_SQLModel>(jsonText);

        Debug.Log("スコア情報(score)：" + userData.scoreList[0].score);


        yield return null;
    }

    //プレイヤーのID番号取得
    public void IDReceive()
    {
        if (networkflag) StartCoroutine(IDReceiveCoroutine());
    }
    IEnumerator IDReceiveCoroutine()
    {
        Debug.Log("ID取得");
        WWW www = new WWW(IDReceiveAdress);
        yield return www;
        userIDstr = www.text;
        Debug.Log("ユーザーID : " + userIDstr);
        yield return null;
    }
    ////////////////↑受信系↑////////////////////////////


    ////////////////↓送信系↓////////////////////////////
    public void Scoresendtest()
    {
        if (networkflag)
        {
            StartCoroutine(ScoreSendtestCoroutine());
        }
    }
    IEnumerator ScoreSendtestCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("userinfo", "Guest");
        form.AddField("date", 0);
        form.AddField("score", 0);
        form.AddField("ID", 0);
        form.AddField("difficulty", 1);//どっかからとってこれるようにする？

        WWW www = new WWW(scoreSendAdress);
        yield return www;

        Debug.Log(www.text);

        yield return null;
    }
    public void Scoresend(G20_GameState state)
    {
        if (networkflag && state==G20_GameState.CLEAR)
        {
            StartCoroutine(ScoreSendCoroutine());
        }
    }
    IEnumerator ScoreSendCoroutine()
    {

        WWWForm form = new WWWForm();
        form.AddField("userinfo", "Guest");
        form.AddField("date", date);
        form.AddField("score", G20_Score.GetInstance().Score);
        form.AddField("ID", userIDstr);
        form.AddField("difficulty", (int)G20_StageManager.GetInstance().stageType);//どっかからとってこれるようにする？

        WWW www = new WWW(scoreSendAdress, form);
        yield return www;

  

        Debug.Log(www.text);

        yield return null;
    }
    ////////////////↑送信系↑////////////////////////////

}



////////////// ↓　モデルクラス　　絶対に触るな！！！！　↓//////////////////
[Serializable] public class G20_SQLModel
{
    public List<G20_ScoreModel> scoreList;
}


[Serializable] public class G20_ScoreModel
{
    public string userinfo;
    public string date;
    public string score;
    public string ID;
    public string difficulty;
}
//////////////↑　モデルクラス　　絶対に触るな！！！！　↑///////////////////