using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_CaptionPerformer : MonoBehaviour
{
    [SerializeField] GameObject bottomBlackBelt;
    [SerializeField] GameObject topBlackBelt;
    [SerializeField] Text captionText;
    [SerializeField] GameObject paramObject;
    [SerializeField] float beltfadeDuration = 1.0f;
    [SerializeField] float beltHeight;
    [SerializeField] float fadeOutDelay=1.0f;
    [SerializeField] float captionFadeDuration;
    //設定位置に表示されている時true
    bool isDisplayed;
    //演出中(コルーチン)が走っている時true
    bool isPerforming;
    // Update is called once per frame

    public void StartPerformance(string caption_message)
    {
        if (isPerforming) return;
        StartCoroutine(CaptionCoroutine(caption_message));
    }
    public void StopPerformance()
    {
        isDisplayed = false;
    }
    IEnumerator CaptionCoroutine(string caption_message)
    {
        isDisplayed = true;
        isPerforming = true;
        captionText.text = caption_message;
        yield return StartCoroutine(MoveBlackBelt(true));
        yield return FadeCaption(true);
        while (isDisplayed)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(fadeOutDelay);
        yield return FadeCaption(false);
        yield return StartCoroutine(MoveBlackBelt(false));
        isPerforming = false;
    }
    IEnumerator MoveBlackBelt(bool isPlus)
    {
        int multiply = isPlus ? 1 : -1;
        for (float t = 0; t <= beltfadeDuration; t += Time.unscaledDeltaTime)
        {
            float movevalue= beltHeight*(1.0f / beltfadeDuration) * Time.unscaledDeltaTime * multiply;
            topBlackBelt.transform.Translate(0,-movevalue , 0);
            bottomBlackBelt.transform.Translate(0, movevalue, 0);
            paramObject.transform.Translate(0, movevalue, 0);
            yield return null;
        }
    }
    IEnumerator FadeCaption(bool isPlus)
    {
        int multiply = isPlus ? 1 : -1;
        for (float t=0;t<=captionFadeDuration;t+= Time.unscaledDeltaTime)
        {
            float value = Time.unscaledDeltaTime * (1.0f / captionFadeDuration)*multiply;
            captionText.color += new Color(0,0,0,value);
            yield return null;
        }
    }
}
