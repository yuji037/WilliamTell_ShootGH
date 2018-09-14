using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class G20_ClearPerformer : G20_Singleton<G20_ClearPerformer>
{
    //この値を変えることでランダムに降らすか整列して降らすかを変えられる
    [SerializeField] bool IsRandomlyFall;
    [SerializeField] GameObject appleObj;
    [SerializeField] Vector3 fallPoint;
    [SerializeField] Vector3 fallSize;
    [SerializeField] float fallTime = 20.0f;
    [SerializeField] float endWaitTime = 10.0f;
    [SerializeField] GameObject paramObjs;


    [SerializeField] GameObject[] onClearDeactivateFieldObjs;
    [SerializeField] GameObject[] onClearActivateFieldObjs;

    [SerializeField] MeshRenderer backGroundMesh;
    [SerializeField] Material clearBackGroundMaterial;

    [SerializeField] Text yourScore;
    [SerializeField] Text creatorsHighScore;
    [SerializeField] Text dailyHighScore;

    [SerializeField] GameObject clearTexts;

    [SerializeField] GameObject playerObj;



    [SerializeField] GameObject highScore;
    [SerializeField] Text highScoreText;
    Animator clearFadeanimator;

    float balanceNum;
    public void Excute(Action on_end_action)
    {
        StartCoroutine(FallAppleRoutine(on_end_action));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(fallPoint, fallSize * 2);
    }
    IEnumerator FallAppleRoutine(Action on_end_action)
    {
        G20_FadeChanger.GetInstance().StartWhiteFadeOut(2.0f);
        yield return new WaitForSeconds(4.0f);
        SetClearObjects();
        playerObj.transform.position = new Vector3(0, 1f, -9.5f);
        playerObj.transform.eulerAngles = Vector3.zero;
        yield return new WaitForSeconds(1f);
        G20_BGMManager.GetInstance().Play(G20_BGMType.CLEAR);
        G20_FadeChanger.GetInstance().StartWhiteFadeIn(2.0f);
        yield return new WaitForSeconds(2.0f);

        var apple_value = G20_Score.GetInstance().Score;
        StartCoroutine(UIRoutine(apple_value));

        //リンゴ積み上げ
        balanceNum = -fallSize.x;
        var fallAppleDelay = fallTime / apple_value;
        for (int i = 0; i < apple_value; i++)
        {
            var apple = Instantiate(appleObj);
            apple.transform.SetParent(transform);
            if (IsRandomlyFall)
            {
                apple.transform.position = GetRandomPosition();
            }
            else
            {
                apple.transform.position = GetBalancePosition();
            }
            yield return new WaitForSeconds(fallAppleDelay);
        }
        yield return new WaitForSeconds(endWaitTime);
        if (on_end_action != null) on_end_action();
    }

    
    IEnumerator UIRoutine(int score)
    {
        SetUIsActive();


        yourScore.text = score.ToString();
        if (G20_NetworkManager.GetInstance().is_network)
        {
            creatorsHighScore.text = "110";//後日入れるかどうするか悩み中
            dailyHighScore.text = G20_NetworkManager.GetInstance().userData.scoreList[0].score;
        }
        else
        {
            creatorsHighScore.text = "110";
            dailyHighScore.text = "110";
        }
        
        yield return new WaitForSeconds(6.5f);

        int num = int.Parse(dailyHighScore.text);

        if (num < score)
        {
            highScore.SetActive(true);
            highScoreText.text = score.ToString();
            yield return null;

            clearFadeanimator = clearTexts.GetComponent<Animator>();
            clearFadeanimator.SetBool("newHighScore", true);

        }

        yield return null;
    }


    //ホワイトアウトしている間にする処理
    void SetClearObjects()
    {
        paramObjs.SetActive(false);
        backGroundMesh.material = clearBackGroundMaterial;
        backGroundMesh.enabled = true;

        foreach (var obj in onClearDeactivateFieldObjs)
        {
            obj.SetActive(false);
        }
        foreach (var obj in onClearActivateFieldObjs)
        {
            obj.SetActive(true);
        }
    }


    void SetUIsActive()
    {
        clearTexts.SetActive(true);
    }
    Vector3 GetBalancePosition()
    {
        Vector3 retPos = fallPoint + new Vector3(balanceNum, 0, 0);
        balanceNum += 0.5f;
        if (balanceNum > fallSize.x)
        {
            balanceNum = -fallSize.x;
        }
        return retPos;
    }
    Vector3 GetRandomPosition()
    {
        var randPos = new Vector3(UnityEngine.Random.Range(-fallSize.x, fallSize.x), UnityEngine.Random.Range(-fallSize.y, fallSize.y), UnityEngine.Random.Range(-fallSize.z, fallSize.z));
        return randPos + fallPoint;
    }
}
