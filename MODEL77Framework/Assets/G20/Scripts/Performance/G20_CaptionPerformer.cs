using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct G20_CaptionParam
{
    public float beltFadeDuration;
    public float captionFadeDuration;
    public float fadeOutDelay;
    public G20_CaptionParam(float beltFadeDuration, float captionFadeDuration, float fadeOutDelay)
    {
        this.beltFadeDuration = beltFadeDuration;
        this.captionFadeDuration = captionFadeDuration;
        this.fadeOutDelay = fadeOutDelay;
    }
}
public class G20_CaptionPerformer : MonoBehaviour
{
    [SerializeField] float beltHeight;
    [SerializeField] GameObject bottomBlackBelt;
    [SerializeField] GameObject topBlackBelt;
    [SerializeField] Text captionText;
    [SerializeField] CanvasGroup paramGroup;
    Vector3 topDefaultPosition;
    Vector3 bottomDefaultPosition;

    //設定位置に表示されている時true
    bool isDisplayed;
    //演出中(コルーチン)が走っている時true
    bool isPerforming;
    // Update is called once per frame
    private void Awake()
    {
        topDefaultPosition = topBlackBelt.transform.position;
        bottomDefaultPosition = bottomBlackBelt.transform.position;
    }
    public void StartPerformance(string caption_message, G20_CaptionParam caption_param=new G20_CaptionParam())
    {
        if (isPerforming) return;
        StartCoroutine(CaptionCoroutine(caption_message, caption_param));
    }
    public void StopPerformance()
    {
        isDisplayed = false;
    }
    IEnumerator CaptionCoroutine(string caption_message, G20_CaptionParam caption_param)
    {
        isDisplayed = true;
        isPerforming = true;
        captionText.text = caption_message;
        yield return StartCoroutine(MoveBlackBelt(true, caption_param.beltFadeDuration));
        yield return FadeCaption(true, caption_param.captionFadeDuration);
        while (isDisplayed)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(caption_param.fadeOutDelay);
        yield return FadeCaption(false, caption_param.captionFadeDuration);
        yield return StartCoroutine(MoveBlackBelt(false, caption_param.beltFadeDuration));
        isPerforming = false;
    }
    IEnumerator MoveBlackBelt(bool isFadeIn, float _duration)
    {
        int multiply = isFadeIn ? 1 : -1;
        for (float t = 0; t <= _duration && (_duration > 0); t += Time.unscaledDeltaTime)
        {
            float movevalue = beltHeight * (1.0f / _duration) * Time.unscaledDeltaTime * multiply;
            topBlackBelt.transform.Translate(0, -movevalue, 0);
            bottomBlackBelt.transform.Translate(0, movevalue, 0);
            yield return null;
        }

        //指定のpositionに必ず補正し、ズレを無くす
        if (isFadeIn)
        {
            topBlackBelt.transform.position = topDefaultPosition + new Vector3(0, -beltHeight, 0); ;
            bottomBlackBelt.transform.position = bottomDefaultPosition + new Vector3(0, beltHeight, 0);
            paramGroup.alpha = 0f;
        }
        else
        {
            topBlackBelt.transform.position = topDefaultPosition;
            bottomBlackBelt.transform.position = bottomDefaultPosition;
            paramGroup.alpha = 1.0f;
        }
    }
    IEnumerator FadeCaption(bool isFadeIn, float _duration)
    {
        int multiply = isFadeIn ? 1 : -1;
        for (float t = 0; t <= _duration && (_duration > 0); t += Time.unscaledDeltaTime)
        {
            float value = Time.unscaledDeltaTime * (1.0f / _duration) * multiply;
            captionText.color += new Color(0, 0, 0, value);
            yield return null;
        }
        //最後に補正
        if (isFadeIn)
        {
            captionText.color += new Color(0, 0, 0, 1);
        }
        else
        {
            captionText.color += new Color(0, 0, 0, -1);
        }
    }
}
