using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class G20_FPSSaver : MonoBehaviour {
    [SerializeField] bool isSaving;
    [SerializeField] G20_FPSCounter counter;
    [SerializeField] string logPath;
    private void Awake()
    {
        G20_GameManager.GetInstance().OnGameEndAction += SaveFPS;
    }
    void SaveFPS()
    {
        if (isSaving)
        {
            string logData = System.DateTime.Now.ToString() + "  " + "FPS:" + counter.GetGameFPS();
            if (G20_NetworkManager.GetInstance().is_network)
            {
                G20_NetworkManager.GetInstance().FpsLogSend(logData);
            }
            else
            {
                //後でネットワーク
                StreamWriter sw;
                FileInfo fi;
                fi = new FileInfo(Application.dataPath + logPath + "/FPSLog.csv");
                sw = fi.AppendText();
                sw.WriteLine(logData);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
