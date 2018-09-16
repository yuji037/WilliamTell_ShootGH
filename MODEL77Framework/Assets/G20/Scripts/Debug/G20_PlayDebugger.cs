﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_PlayDebugger : MonoBehaviour
{
    [SerializeField] G20_Player player;
    [SerializeField] G20_EnemyPopper popper;
    [SerializeField] GameObject[] ActiveObjects;
    [SerializeField] GameObject[] DeActiveObjects;
    [SerializeField] Text AIMText;
    [SerializeField] Text EnemySpeedText;
    [SerializeField] Text ShotCountText;
    [SerializeField] Text Description;
    [SerializeField] Text LogText;
    Coroutine logCoroutine;
    bool DebugActive;
    G20_DebugAutoShooter autoShooter;
    private void Awake()
    {
        DebugActivate(false);
        if (Description) Description.text ="F3(Debug)"+"\n"
                +"I(Invin)" + "\n"
                  + "C(Clear)"+"\n"
                  + "U(UpScore)" + "\n"
                  + "G(UpGoldPoint)" + "\n"
                  + "S(ShotCheat)" + "\n"
                  + "←(EneSpe)→" + "\n";
        autoShooter = G20_ComponentUtility.FindComponentOnScene<G20_DebugAutoShooter>();
    }
    void ShowLog(string _log, float duration_time)
    {
        if (logCoroutine!=null)
        {
            StopCoroutine(logCoroutine);
        }
        logCoroutine=StartCoroutine(ShowLogRoutine(_log, duration_time));
    }
    IEnumerator ShowLogRoutine(string _log,float duration_time)
    {
        LogText.text=_log;
        LogText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration_time);
        LogText.gameObject.SetActive(false);
    }
    void DebugActivate(bool _active)
    {
        DebugActive = _active;
        foreach (var i in ActiveObjects)
        {
            i.SetActive(_active);
        }
        foreach (var i in DeActiveObjects)
        {
            i.SetActive(!_active);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            DebugActivate(!DebugActive);
        }
        if (DebugActive)
        {
            InputInvin();
            InputCheatShoot();
            InputClear();
            InputEnemySpeed();
            InputAIMAssist();
            InputSaveAIM();
            InputPlusScore();
            InputChangeAutoShoot();
        }
    }
    void InputChangeAutoShoot()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            autoShooter.IsAutoShooting = !autoShooter.IsAutoShooting;
        }
    }
    void InputSaveAIM()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            G20_BulletShooter.GetInstance().SaveAIMParam();
            ShowLog("AIMParamをSaveしました",1.0f);
        }
    }
   void InputPlusScore()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            G20_Score.GetInstance().AddScore(10);
        }
        if ( Input.GetKeyDown(KeyCode.G) )
        {
            G20_Score.GetInstance().AddGoldPoint(1);
        }
    }
    void InputEnemySpeed()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            popper.onPopSpeedBuffValue += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            popper.onPopSpeedBuffValue -= 0.1f;
        }
        EnemySpeedText.text = string.Format("{0:0.0}", popper.onPopSpeedBuffValue);

    }
    void InputClear()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (G20_StageManager.GetInstance().nowStageBehaviour)
            {
                G20_StageManager.GetInstance().nowStageBehaviour.EndStage();
            }
        }
    }
    void InputInvin()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            player.IsInvincible = !player.IsInvincible;
        }
    }
    void InputCheatShoot()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            G20_CheatShoot.GetInstance().IsChaeting = !G20_CheatShoot.GetInstance().IsChaeting;
        }
    }
    void InputAIMAssist()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            G20_BulletShooter.GetInstance().param.MaxValue += 20;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            G20_BulletShooter.GetInstance().param.MaxValue -= 20;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            G20_BulletShooter.GetInstance().param.OneChangeValue += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            G20_BulletShooter.GetInstance().param.OneChangeValue -= 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            G20_BulletShooter.GetInstance().param.DefaultValue += 10.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            G20_BulletShooter.GetInstance().param.DefaultValue -= 10.0f;
        }
        AIMText.text = "A : " + G20_BulletShooter.GetInstance().aimAssistValue + "\n"
            + "A(AMAX)D : " + G20_BulletShooter.GetInstance().param.MaxValue + "\n"
            + "1(AOne)2 : " + G20_BulletShooter.GetInstance().param.OneChangeValue + "\n"
            + "3(ADefault)4 : " + G20_BulletShooter.GetInstance().param.DefaultValue + "\n"
            + "Tab(AIMSave)"+"\n";
        ShotCountText.text = "ShotCount : "+G20_BulletShooter.GetInstance().ShotCount;

    }
}
