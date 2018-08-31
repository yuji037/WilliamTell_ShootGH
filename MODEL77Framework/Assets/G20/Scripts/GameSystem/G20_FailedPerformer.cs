using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// unScaledTimeにする
public class G20_FailedPerformer : G20_Singleton<G20_FailedPerformer>
{
    [SerializeField] Text FailedText;
    [SerializeField] Image FailedPanel;
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject[] ActiveObjs;
    [SerializeField] GameObject[] DeActiveObjs;
    [SerializeField] float fadeAlpha = 0.5f;
    [SerializeField] float fadeTime = 3.0f;
    public void Excute(Action on_end_action)
    {
        fadeAlpha= Mathf.Clamp(fadeAlpha, 0, 1.0f); 
        foreach (var i in DeActiveObjs)
        {
            i.SetActive(false);
        }
        foreach (var i in ActiveObjs)
        {
            i.SetActive(true);
        }
        StartCoroutine(FailedTextRoutine(on_end_action));
        StartCoroutine(FailedPanelRoutine());
        ScoreText.text = "SHOT APPLE:" + G20_Score.GetInstance().Score;
        Debug.Log(Time.unscaledDeltaTime);
    }
    IEnumerator FailedTextRoutine(Action on_end_action)
    {
        var fColor = FailedText.color;
        fColor.a = 0;
        FailedText.color = fColor;
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            FailedText.color += new Color(0, 0, 0, Time.unscaledDeltaTime * (1.0f / fadeTime));
            yield return null;
        }
        if (on_end_action != null) on_end_action();
    }
    IEnumerator FailedPanelRoutine()
    {
        var fColor = FailedPanel.color;
        fColor.a = 0;
        FailedPanel.color = fColor;
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            FailedPanel.color += new Color(0, 0, 0, Time.unscaledDeltaTime * (1.0f / fadeTime));
            if (FailedPanel.color.a >= fadeAlpha) break;
            yield return null;
        }
    }

}
