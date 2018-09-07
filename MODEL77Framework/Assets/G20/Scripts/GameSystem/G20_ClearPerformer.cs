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
    [SerializeField] float fallAppleDelay;
    [SerializeField] GameObject paramObjs;

    
    [SerializeField] GameObject[] onClearDeactivateFieldObjs;
    [SerializeField] GameObject[] onClearActivateFieldObjs;

    [SerializeField] MeshRenderer backGroundMesh;
    [SerializeField] Material clearBackGroundMaterial;

    [SerializeField] GameObject skyObject;
    [SerializeField] Light directionLight;
    [SerializeField] GameObject gesuraObj;
    [SerializeField] Light directionLight2;


    [SerializeField] GameObject clearTexts;
    [SerializeField] Image ScorePanel;
    [SerializeField] Text yourScoreText;
    [SerializeField] Text yourScore;
    [SerializeField] Text highScore;
    [SerializeField] Text creators;
    [SerializeField] Text creatorsHighScore;
    [SerializeField] Text daily;
    [SerializeField] Text dailyHighScore;

    
    [SerializeField] float fadetime=0;


    float balanceNum;
    public void Excute(Action on_end_action)
    {
        StartCoroutine(FallAppleRoutine(on_end_action));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(fallPoint,fallSize*2);
    }
    IEnumerator FallAppleRoutine(Action on_end_action)
    {
        G20_FadeChanger.GetInstance().StartWhiteFadeOut(2.0f);
        yield return new WaitForSeconds(2.0f);
        SetClearObjects();
        yield return new WaitForSeconds(0.1f);
        G20_FadeChanger.GetInstance().StartWhiteFadeIn(2.0f);
        yield return new WaitForSeconds(2.0f);
        
        var apple_value = G20_Score.GetInstance().Score;
        StartCoroutine(UIRoutine(apple_value));

        //リンゴ積み上げ
        balanceNum = -fallSize.x;
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
        if (on_end_action != null) on_end_action();
    }


    IEnumerator UIRoutine(int score)
    {
        SetUIsActive();

        yield return new WaitForSeconds(1.0f);

        yourScore.text = score.ToString();
        //creatorsHighScore.text = G20_NetworkManager.GetInstance().userData.scoreList[0].score;
        //dailyHighScore.text    = G20_NetworkManager.GetInstance().userData.scoreList[0].score;
        creatorsHighScore.text = "0";
        dailyHighScore.text = "0";
        yield return null;

        for(float t = 0; t < fadetime; t+=Time.deltaTime)
        {
            ScorePanel.color += new Color(0,0,0,Time.deltaTime * 200 / 255);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f); 

        for(float t = 0; t < fadetime; t += Time.deltaTime)
        {
            yourScoreText.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        for (float t = 0; t < fadetime; t += Time.deltaTime)
        {
            yourScore.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        for (float t = 0; t < fadetime; t += Time.deltaTime)
        {
            creators.color += new Color(0, 0, 0, Time.deltaTime);
            creatorsHighScore.color += new Color(0, 0, 0, Time.deltaTime);
            daily.color += new Color(0, 0, 0, Time.deltaTime);
            dailyHighScore.color += new Color(0, 0, 0, Time.deltaTime);
            highScore.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        

        yield return null;
    }


    //ホワイトアウトしている間にする処理
    void SetClearObjects()
    {
        paramObjs.SetActive(false);
        backGroundMesh.material = clearBackGroundMaterial;

        foreach ( var obj in onClearDeactivateFieldObjs )
        {
            obj.SetActive(false);
        }
        foreach ( var obj in onClearActivateFieldObjs )
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
        Vector3 retPos = fallPoint + new Vector3(balanceNum,0,0);
        balanceNum+=0.5f;
        if (balanceNum>fallSize.x)
        {
            balanceNum = -fallSize.x;
        }
        return retPos;
    }
    Vector3 GetRandomPosition()
    {
        var randPos = new Vector3( UnityEngine.Random.Range(-fallSize.x, fallSize.x), UnityEngine.Random.Range (- fallSize.y, fallSize.y), UnityEngine.Random.Range( - fallSize.z, fallSize.z));
        return randPos+fallPoint;
    }
}
