using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_FPSCounter : MonoBehaviour
{
    [SerializeField]float CalcDuration;
    [SerializeField] Text fpsText;
    [SerializeField]float showFPSDuration;
    Coroutine fpsCoroutine;
    // Use this for initialization
    void Awake()
    {
        StartCoroutine(CountFPS());
    }
    IEnumerator CountFPS()
    {
        while (true)
        {
            int frameCount = 0;
            float timer = 0f;
            while (CalcDuration >= timer)
            {
                frameCount++;
                timer += Time.deltaTime;
                yield return null;
            }
            //FPS算出
            float fps=(frameCount / timer);
            if (fps<60.0f)
            {
                fpsText.text = "FPS:" +(int)fps;
                //前のコルーチンを削除
                if (fpsCoroutine != null) StopCoroutine(fpsCoroutine);
                //表示コルーチン
                fpsCoroutine =StartCoroutine(ShowFPS(fps));
            }
        }
    }
    IEnumerator ShowFPS(float _fps)
    {
        fpsText.enabled = true;
        yield return new WaitForSeconds(showFPSDuration);
        fpsText.enabled = false;
    }
}
