using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_FPSCounter : MonoBehaviour
{
    [SerializeField] float CalcDuration;
    [SerializeField] Text fpsText;
    [SerializeField] float showFPSDuration;
    [SerializeField] bool isShowFPS;
    Coroutine fpsCoroutine;
    float gameStartTime=0f;
    int gameFrameCount = 0;
    public float GetGameFPS()
    {
        var gameTime = Time.time - gameStartTime;
        return((float)gameFrameCount / gameTime);
    }
    // Use this for initialization
    void Awake()
    {
        StartCoroutine(CountFPS());
    }
    private void Update()
    {
        gameFrameCount++;
    }
    IEnumerator CountFPS()
    {
        gameStartTime = Time.time;
        //Debug.Log(gameStartTime);
        while (true)
        {
            int frameCount = 0;
            float timer = 0f;
            while (CalcDuration >= timer)
            {
                frameCount++;
                timer += Time.unscaledDeltaTime;
                yield return null;
                //FPS算出
                float fps = (frameCount / timer);
               if(isShowFPS)ShowFPS(fps);
            }

        }
    }
    void ShowFPS(float _fps)
    {
        if (_fps < 60.0f)
        {
            fpsText.text = "FPS:" + (int)_fps;
            //前のコルーチンを削除
            if (fpsCoroutine != null) StopCoroutine(fpsCoroutine);
            //表示コルーチン
            fpsCoroutine = StartCoroutine(ShowRoutine(_fps));
        }
    }
    IEnumerator ShowRoutine(float _fps)
    {
        fpsText.enabled = true;
        yield return new WaitForSeconds(showFPSDuration);
        fpsText.enabled = false;
    }
}
