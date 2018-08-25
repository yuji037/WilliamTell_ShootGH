using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_FadeChanger : G20_Singleton<G20_FadeChanger>
{
    [SerializeField] Image fadePanel;
    Coroutine currentRoutine;
    public void StartBlackFadeIn(float take_time)
    {
        StartFadeIn(Color.black, take_time);
    }
    public void StartBlackFadeOut(float take_time)
    {
        StartFadeOut(Color.black, take_time);
    }
    public void StartWhiteFadeIn(float take_time)
    {
        StartFadeIn(Color.white, take_time);
    }
    public void StartWhiteFadeOut(float take_time)
    {
        StartFadeOut(Color.white, take_time);
    }

    //自分で色の値を設定できる
    public void StartFadeIn(Color _color, float take_time)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeInRoutine(_color, take_time));
    }
    //自分で色の値を設定できる
    public void StartFadeOut(Color _color, float take_time)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeOutRoutine(_color, take_time));
    }

    IEnumerator FadeInRoutine(Color _color, float take_time)
    {
        fadePanel.color = _color;
        yield return StartCoroutine(FadeRoutine(take_time, false));
    }
    IEnumerator FadeOutRoutine(Color _color, float take_time)
    {
        _color.a = 0;
        fadePanel.color = _color;
        yield return StartCoroutine(FadeRoutine(take_time, true));
    }
    IEnumerator FadeRoutine(float take_time, bool is_plus)
    {
        int multipliValue = 1;
        if (!is_plus) multipliValue = -1;
        for (float t = 0; t < take_time; t += Time.deltaTime)
        {
            fadePanel.color += multipliValue * new Color(0, 0, 0, (1.0f / take_time) * Time.deltaTime);
            yield return null;
        }

    }
}
