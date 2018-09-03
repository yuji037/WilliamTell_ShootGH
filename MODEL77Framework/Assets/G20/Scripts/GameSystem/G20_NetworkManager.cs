using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_NetworkManager : MonoBehaviour {
    [SerializeField] string adress = "http://";
    [SerializeField] string ip = "127.0.0.1:10080";
    [SerializeField] string dir = "/gp17op17/WT/";

    [SerializeField] string file = "ScoreSend.php";

    string scoresendadress;

    [SerializeField] bool networkflag = false;
    // Use this for initialization
    void Start () {
        scoresendadress = adress + ip + dir + file;
        if (networkflag) StartCoroutine(Scorereceive());
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Scorereceive()
    {
        WWW www = new WWW(adress + ip + dir + file);
        yield return www;

        Debug.Log(www.text);

        yield return null;
    }
}
