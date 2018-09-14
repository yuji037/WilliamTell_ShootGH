using System.Collections;
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
    [SerializeField] Text InvinText;
    [SerializeField] Text CheetShotText;
    [SerializeField] Text Description;
    bool DebugActive;

    private void Awake()
    {
        DebugActivate(false);
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
        }
    }
    void InputSaveAIM()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            G20_BulletShooter.GetInstance().SaveAIMParam();
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
            G20_BulletShooter.GetInstance().isCheating = !G20_BulletShooter.GetInstance().isCheating;
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
            + "AMAX : " + G20_BulletShooter.GetInstance().param.MaxValue + "\n" 
            + "AOne : " + G20_BulletShooter.GetInstance().param.OneChangeValue + "\n"
            + "ADefault : " + G20_BulletShooter.GetInstance().param.DefaultValue+"\n";

    }
}
