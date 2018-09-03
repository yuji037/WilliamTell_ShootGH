using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_NetworkManager : MonoBehaviour {
    [SerializeField] string adress = "http://";
    [SerializeField] string ip = "127.0.0.1:10080";
    [SerializeField] string dir = "/gp17op17/WT/";



    string scoreSendAdress;
    string scoreReceiveAdress;

    [SerializeField] bool networkflag = false;
    // Use this for initialization
    void Start () {
        scoreReceiveAdress = adress + ip + dir + "ScoreSend.php";
        scoreSendAdress = adress + ip + dir + "ScoreReceive.php";

        if (networkflag) StartCoroutine(Scorereceive());
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Scorereceive()
    {
        WWW www = new WWW(scoreReceiveAdress);
        yield return www;

        Debug.Log(www.text);

        yield return null;
    }

    IEnumerator scoreSend()
    {
        WWW www = new WWW(scoreReceiveAdress);
        yield return www;

        yield return null;
    }
}
