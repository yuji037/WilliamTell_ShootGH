﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class G20_ClearPerformer : G20_Singleton<G20_ClearPerformer>
{
    //この値を変えることでランダムに降らすか整列して降らすかを変えられる
    [SerializeField] bool IsRandomlyFall;
    [SerializeField] GameObject appleObj;
    [SerializeField] GameObject goldenAppleObj;
    [SerializeField] GameObject smallEneAppleObj;
    //1/smallEneRateの確率で出現
    [SerializeField] int smallEneRate;
    [SerializeField] Vector3 fallPoint;
    [SerializeField] Vector3 fallSize;
    public float fallTime = 20.0f;
    public float endWaitTime = 10.0f;
    [SerializeField] GameObject paramObjs;
    [SerializeField] G20_ScoreCalcData scoreCalcData;

    [SerializeField] GameObject[] onClearDeactivateFieldObjs;
    [SerializeField] GameObject[] onClearActivateFieldObjs;

    [SerializeField] MeshRenderer backGroundMesh;
    [SerializeField] Material clearBackGroundMaterial;

    [SerializeField] Text yourScore;
    [SerializeField] Text yourScore_copy;
    //[SerializeField] Text creatorsHighScore;
    //[SerializeField] Text dailyHighScore;

    [SerializeField] GameObject clearTexts;

    [SerializeField] GameObject playerObj;


    [SerializeField] GameObject highScore;
    [SerializeField] Text highScoreText;
    [SerializeField] Text ChainText;
    [SerializeField] Text HitRateText;
    Animator clearFadeanimator;

    public int scorecount = 0;
    float balanceNum;
    int GetHitRateBonus()
    {
        return scoreCalcData.HitRateMultyply * (int)(G20_BulletShooter.GetInstance().HitRate*100);
    }
    int GetMaxChainBonus()
    {
        return scoreCalcData.MaxChainMultiply * G20_ChainCounter.GetInstance().MaxChainCount;
    }
    public void Excute(Action on_end_action)
    {
        StartCoroutine(FallAppleRoutine(on_end_action));
    }
    IEnumerator FallAppleRoutine(Action on_end_action)
    {
        G20_FadeChanger.GetInstance().StartWhiteFadeOut(2.0f);
        yield return new WaitForSeconds(2.0f);
        SetClearObjects();
        playerObj.transform.position = new Vector3(0, 1f, -9.5f);
        playerObj.transform.eulerAngles = Vector3.zero;
        yield return new WaitForSeconds(1f);
        G20_BGMManager.GetInstance().Play(G20_BGMType.CLEAR);
        G20_FadeChanger.GetInstance().StartWhiteFadeIn(2.0f);
        yield return new WaitForSeconds(2.0f);
       
        //スコア算出
        var hitRateScore= GetHitRateBonus();
        HitRateText.text = hitRateScore.ToString();

        var maxChainScore = GetMaxChainBonus();
        ChainText.text = maxChainScore.ToString();

        var sumScore = G20_ScoreManager.GetInstance().GetSumScore()+maxChainScore+hitRateScore;
        
        // リンゴ落下数の計算
        var goldFallCount = G20_ScoreManager.GetInstance().GoldPoint.Value;
        // 全リンゴ
        var totalAppleCount = (sumScore / 100) - goldFallCount * 2;
        // 全リンゴ25、金4のとき
        // 5,11,17,23番目に落とす（6の倍数-1）
        // 全リンゴ25、金5のとき
        // 4,9,14,19,24番目に落とす（5の倍数-1）
        int goldRate = 9999;
        if (goldFallCount != 0) goldRate = totalAppleCount / goldFallCount;

        initUI();

        //リンゴ積み上げ
        balanceNum = -fallSize.x;
        var fallAppleDelay = fallTime / totalAppleCount;
        for (int i = 0; i < totalAppleCount; i++)
        {
            GameObject apple;
            bool isGoldenApple = (i % goldRate == goldRate - 1);
            bool isSmalleEne = (UnityEngine.Random.Range(0, smallEneRate) == 1);
            if (isGoldenApple) apple = Instantiate(goldenAppleObj);
            else if (isSmalleEne) apple = Instantiate(smallEneAppleObj);
            else apple = Instantiate(appleObj);

            var fallAppleSound = apple.GetComponent<G20_FallAppleSound>();
            fallAppleSound.firstCollisionHItAction += PlusAppleScore;
            fallAppleSound.eventArgInteger = isGoldenApple ? 300 : 100;
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

        float timer = 0f;

        do
        {
            timer += Time.deltaTime;
            yield return null;
            //全てリンゴが落ちたら合計スコアを代入
            if (totalAppleCount == cuurentFellCount)
            {
                yourScore_copy.text = sumScore.ToString();
                yourScore.text = sumScore.ToString();
                Debug.Log("チェンジ" + sumScore);
            }
        } while ((totalAppleCount != cuurentFellCount) && (timer <= endWaitTime));
        if (on_end_action != null) on_end_action();
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

    void initUI()
    {
        
        yourScore.text = G20_ScoreManager.GetInstance().GetSumScore().ToString();
        ChainText.text = G20_ChainCounter.GetInstance().MaxChainCount.ToString();
        HitRateText.text = G20_BulletShooter.GetInstance().HitRate.ToString();
        SetUIsActive();

    }
    //スコアアップルが落ちた回数
    int cuurentFellCount;

    //一番最後にスコアを直接いれたほうがいいかも
    void PlusAppleScore(int addValue)
    {
       
    }


}





