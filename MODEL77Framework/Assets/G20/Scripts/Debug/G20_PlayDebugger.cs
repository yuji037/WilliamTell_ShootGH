using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_PlayDebugger : MonoBehaviour
{
    [SerializeField] G20_Player player;
    [SerializeField] G20_HitDebugToggle invinObj;
    [SerializeField] G20_HitDebugToggle cheatShotObj;
    [SerializeField] G20_HitEmpty replayObj;
    [SerializeField] G20_HitEmpty minusSpeed;
    [SerializeField] G20_HitEmpty plusSpeed;
    [SerializeField] TextMesh speedTextMesh;
    [SerializeField] G20_EnemyPopper popper;
    [SerializeField] G20_HitDebugToggle debugObjToggle;
    [SerializeField] GameObject[] ActiveObjects;
    [SerializeField] GameObject[] DeActiveObjects;

    [SerializeField] G20_HitEmpty stageSelectObj;
    [SerializeField] TextMesh stageNameTextMesh;
    [SerializeField] GameObject[] stagePrefabs;
    G20_StageManager stageManager;
    int stageIndex = 0;

    [SerializeField] G20_HitDebugToggle skipObj;

    private void Awake()
    {
        invinObj.toggleAction += ChangePlayerInvin;
        cheatShotObj.toggleAction += ChangeCheatShoot;
        skipObj.toggleAction += ChangeIsSkipIngamePerformance;
        debugObjToggle.toggleAction += ChangeAllActive;
        replayObj.hitAction += () => { Time.timeScale = 1.0f; G20_ReloadScene.GetInstance().ReloadScene(); };
        minusSpeed.hitAction += () =>
         {
             popper.onPopSpeedBuffValue -= 0.1f;
             speedTextMesh.text = string.Format("{0:0.0}", popper.onPopSpeedBuffValue);
         };
        plusSpeed.hitAction += () =>
        {
            popper.onPopSpeedBuffValue += 0.1f;
            speedTextMesh.text = string.Format("{0:0.0}", popper.onPopSpeedBuffValue);
        };
        speedTextMesh.text = string.Format("{0:0.0}", popper.onPopSpeedBuffValue);

        stageManager = G20_StageManager.GetInstance();
        stageSelectObj.hitAction += () =>
        {
            stageIndex++;
            if ( stageIndex >= stagePrefabs.Length ) stageIndex = 0;
            stageManager.stageBehaviourPrefab = stagePrefabs[stageIndex];
            UpdateStageNameTextMesh();
        };
        UpdateStageNameTextMesh();
    }
    void ChangeAllActive(bool _active)
    {
            foreach (var i in ActiveObjects)
            {
                i.SetActive(_active);
            }
            foreach (var i in DeActiveObjects)
            {
                i.SetActive(!_active);
            }
     
        if (_active)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    void ChangeIsSkipIngamePerformance(bool _active)
    {
        G20_GameManager.GetInstance().isSkipPerformance = _active;
    }

    void UpdateStageNameTextMesh()
    {
        var name = stageManager.stageBehaviourPrefab.name;
        name = name.Substring(9);
        stageNameTextMesh.text = name;
    }

    void ChangePlayerInvin(bool _active)
    {
        player.IsInvincible = _active;
    }
    void ChangeCheatShoot(bool _active)
    {
        G20_BulletShooter.GetInstance().isCheating = _active;
    }
}
