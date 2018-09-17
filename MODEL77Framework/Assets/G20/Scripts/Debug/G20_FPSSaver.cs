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
            G20_NetworkManager.GetInstance().FpsLogSend(logData);
        }
    }
}
