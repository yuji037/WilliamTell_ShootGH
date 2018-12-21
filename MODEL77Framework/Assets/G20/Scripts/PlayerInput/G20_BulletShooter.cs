using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class G20_AIMParam
{
    //初期値
    public float DefaultValue;
    //最大値
    public float MaxValue;
    //一発毎の偏移値
    public float OneChangeValue;
}

//hitObjectのactionを起動するclass
public class G20_BulletShooter : G20_Singleton<G20_BulletShooter>
{
    [SerializeField] Camera effectCamera;
    [SerializeField] LayerMask panelMask;
    [SerializeField] LayerMask fieldMask;
    //AIMのパラメーター
   public G20_AIMParam param=new G20_AIMParam();
    //trueの時弾を発射
    public bool CanShoot = true;
    //AIM補正の値
    public float aimAssistValue = 0f;
    //HitObjectに弾が当たった時のAction
    public event Action<G20_HitObject> ActionHitObject;
    //プレイヤーのInput以外で着弾座標を決める時に実行するFunc(例：自動射撃など)
    Func<Vector2?> GetShtPointFunc;
    public void ChangeGetShotPointFunc(Func<Vector2?> getShtPointFunc)
    {
        GetShtPointFunc = getShtPointFunc;
    }
    public void RemoveGetShotPointFunc()
    {
        GetShtPointFunc = null;
    }
    public int ShotCount { get; private set; }
    public int AssistHitCount { get; private set;}
    public float HitRate { get; private set; }
    public bool coutingHitRate=true;

    public void SaveAIMParam()
    {
        PlayerPrefs.SetFloat("G20_AIMMax", param.MaxValue);
        PlayerPrefs.SetFloat("G20_AIMDefault", param.DefaultValue);
        PlayerPrefs.SetFloat("G20_AIMOneChange", param.OneChangeValue);
        PlayerPrefs.Save();
        //Debug.Log("AIMパラメーターをセーブしました。");
    }
    public void LoadAIMParam()
    {
       param.MaxValue =PlayerPrefs.GetFloat("G20_AIMMax",100);
       param.DefaultValue= PlayerPrefs.GetFloat("G20_AIMDefault", 40);
       param.OneChangeValue= PlayerPrefs.GetFloat("G20_AIMOneChange", 3);
    }
    
    private void Start()
    {
        LoadAIMParam();
        aimAssistValue = param.DefaultValue;
    }
    private void Update()
    {
        Vector2? shootPoint = G20_InputPointGetter.GetInstance().GetInputPoint();

        if (!CanShoot) return;

        if (shootPoint==null&& GetShtPointFunc != null)
        {
            shootPoint = GetShtPointFunc();
        }

        if (shootPoint != null)
        {
            ShotCount++;
            if (aimAssistValue > 0)
            {
                //AIM補正の値をshotPointに代入
                shootPoint = G20_AIMAssistant.AssistAIM((Vector2)shootPoint, aimAssistValue);
            }

            var hitObject=ShootHitObject((Vector2)shootPoint);
            if (hitObject&&hitObject.IsHitRateUp)
            {
                AssistHitCount++;
            }
            if (coutingHitRate)
            {
                HitRate = (float)AssistHitCount / (float)ShotCount;
            }
            ShootPanel((Vector2)shootPoint);

        }
    }
    G20_HitObject ShootHitObject(Vector2 shootPoint)
    {
        Vector3 hitPoint = Vector3.zero;
        var hitObject = G20_RayShooter.GetHitObject<G20_HitObject>(shootPoint, ref hitPoint, Camera.main, fieldMask);
        //aimAssistの値を増減
        AdjustAIMAssist(hitObject);
        if (ActionHitObject != null) ActionHitObject(hitObject);
        if (hitObject)
        {
            hitObject.ExcuteActions(hitPoint);
        }
        return hitObject;
    }
    G20_HitObject ShootPanel(Vector2 shootPoint)
    {
        Vector3 panelhitPoint = Vector3.zero;
        var hitPanel = G20_RayShooter.GetHitObject<G20_HitObject>(shootPoint, ref panelhitPoint, effectCamera, panelMask);
        if (hitPanel)
        {
            hitPanel.ExcuteActions(panelhitPoint);
        }
        return hitPanel;
    }
    void AdjustAIMAssist(G20_HitObject hitObject)
    {
        if (hitObject && (hitObject.HitTag == G20_HitTag.ASSIST))
        {
            aimAssistValue -= param.OneChangeValue;
        }
        else
        {
            aimAssistValue += param.OneChangeValue;
        }
        aimAssistValue = Mathf.Clamp(aimAssistValue, 0, param.MaxValue);
    }
    private void OnDestroy()
    {
        GetShtPointFunc = null;
        base.OnDestroy();
    }
}
