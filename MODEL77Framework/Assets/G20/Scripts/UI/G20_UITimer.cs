using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_UITimer : MonoBehaviour {

    [SerializeField] Text UItext;
    [SerializeField] Slider UISlider;
    [SerializeField] Text UItext2;
    [SerializeField] Image UIDonatu;
	// Use this for initialization
	void Update () {
        ApplyTimer(G20_Timer.GetInstance().CurrentTime);
	}
	
	// Update is called once per frame
	void ApplyTimer(float _timer)
    {
        float timeRate= G20_Timer.GetInstance().CurrentTime / G20_Timer.GetInstance().FirstTime;
        UISlider.value = timeRate;
        UIDonatu.fillAmount = timeRate;

        string timeStr = string.Format("{0:0.0}", _timer);
        UItext.text = timeStr;
        UItext2.text = timeStr;
    }
}
