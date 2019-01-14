using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class G20_NetworkManager : G20_Singleton<G20_NetworkManager>
{
    //TGS用のクリエイタースコア
    [NonSerialized]public int[] creatorScore=new int[2];

    public G20_SQLModel userData;

    string userIDstr;

    [SerializeField] string adress = "http://";
    [SerializeField,Header("ビルド時は空にする")] string ip = "192.168.40.129";
    [SerializeField] string dir = "/WT/";
    [SerializeField] string scoreSendFile = "ScoreReceive.php";
    [SerializeField] string scoreReceiveFile = "ScoreSend.php";
    [SerializeField] string IDReceiveFile = "IDSend.php";
    string FPSSendFile = "FPSlogReceive.php";

    string scoreSendAdress;
    string scoreReceiveAdress;
    string IDReceiveAdress;
    string FPSSendAdress;


    int date;

    public bool is_network = false;
    // Use this for initialization
    void Start()
    {

        if (is_network)
        {
            IPReceive();
            G20_GameManager.GetInstance().ChangedStateAction += Scoresend;
            G20_GameManager.GetInstance().ChangedStateAction += Scorereceive;
        }
        
    }

    ////////////////↓受信系↓///////////////////////////////////////////////////
    void IPReceive()
    {
        StartCoroutine(IPReceiveCoroutine());
    }

    IEnumerator IPReceiveCoroutine()
    {
       
        WWW www = new WWW("http://api.max-bullet.com/wt/");
        yield return www;

        //Debug.Log("接続先IP : " + www.text);
        if (!string.IsNullOrEmpty(www.error))
        {
            is_network = false;
            //Debug.Log("ネットワークエラー");
            yield break;
        }

        yield return null;

        if (string.IsNullOrEmpty(ip))
        {
            ip = www.text;
        }
        

        scoreSendAdress = adress + ip + dir + scoreSendFile;
        scoreReceiveAdress = adress + ip + dir + scoreReceiveFile;
        IDReceiveAdress = adress + ip + dir + IDReceiveFile;
        FPSSendAdress = adress + ip + dir + FPSSendFile;
        //Debug.Log("スコア送信アドレス : " + scoreSendAdress);
        //Debug.Log("スコア受信アドレス : " + scoreReceiveAdress);
        //Debug.Log("ID受信アドレス : " + IDReceiveAdress);
        //Debug.Log("FPSLog送信アドレス : " + FPSSendAdress);

        date = DateTime.Now.Month * 100 + DateTime.Now.Day;
        yield return null;
        IDReceive();
        
      
    }


    //ランキング用スコアの取得
    public void Scorereceive(G20_GameState state)
    {
        if (state == G20_GameState.INGAME)
        {
            StartCoroutine(ScoreReceiveCoroutine());
        }
    }
    //難易度、その日の最高スコア取得
    IEnumerator ScoreReceiveCoroutine()
    {
        //Debug.Log("スコア表受信開始 " 
        //    +"\n日付："+date
        //    +"\n難易度：" + G20_StageManager.GetInstance().stageType +"("+ (int)G20_StageManager.GetInstance().stageType+")");

        WWWForm form = new WWWForm();
        form.AddField("date", date);
        form.AddField("difficulty", G20_GameManager.GetInstance().gameDifficulty);

        WWW www = new WWW(scoreReceiveAdress, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            is_network = false;
            //Debug.Log("ネットワークエラー");
            yield break;
        }

        string jsonText = "{ \"scoreList\" : " + www.text + "}";

        //Debug.Log("jsonText" + jsonText);

        userData = JsonUtility.FromJson<G20_SQLModel>(jsonText);
        
        yield return null;
    }

    //プレイヤーのID番号取得
    public void IDReceive()
    {
        StartCoroutine(IDReceiveCoroutine());
    }
    IEnumerator IDReceiveCoroutine()
    {
        //Debug.Log("ID取得");
        WWW www = new WWW(IDReceiveAdress);
        yield return www;


        if (!string.IsNullOrEmpty(www.error))
        {
            is_network = false;
            //Debug.Log("ネットワークエラー");
            yield break;
        }
        userIDstr = www.text;

        //Debug.Log("ユーザーID : " + userIDstr);
        yield return null;
    }
    ////////////////↑受信系↑////////////////////////////


    ////////////////↓送信系↓////////////////////////////
    //クリア後スコアを送信
    public void Scoresend(G20_GameState state)
    {
        if (state == G20_GameState.CLEAR)
        {
            StartCoroutine(ScoreSendCoroutine());
        }
    }
    IEnumerator ScoreSendCoroutine()
    {
        if (!is_network) yield break;

        WWWForm form = new WWWForm();
        form.AddField("userinfo", "Guest");
        form.AddField("date", date);
        form.AddField("score", G20_ScoreManager.GetInstance().GetSumScore());
        form.AddField("ID", userIDstr);
        form.AddField("difficulty", (int)G20_GameManager.GetInstance().gameDifficulty);//どっかからとってこれるようにする？

        WWW www = new WWW(scoreSendAdress, form);
        yield return www;
        
        //Debug.Log(www.text);

        yield return null;
    }


    public void FpsLogSend(string logtext)
    {
        if (is_network)
        {
            StartCoroutine(FpslogSendCol(logtext));
        }
    }

    IEnumerator FpslogSendCol(string sendtext)
    {
        if (!is_network) yield break;

        WWWForm form = new WWWForm();
        form.AddField("fpslog", sendtext);

        WWW www = new WWW(FPSSendAdress, form);
        yield return www;

        //Debug.Log("sendOK?" + www.text);
    }
    ////////////////↑送信系↑////////////////////////////

}

////////////// ↓　モデルクラス　　絶対に触るな！！！！　↓//////////////////
//触ると通信できません
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