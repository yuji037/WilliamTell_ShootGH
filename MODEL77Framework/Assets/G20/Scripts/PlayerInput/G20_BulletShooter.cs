using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    //弾の射出を制限出来る
    public bool CanShoot = true;
    //チートモード
    public bool isCheating;
    //AIM補正の値
    public float aimAssistValue = 0f;
    int shotCount;
    public int ShotCount
    {
        get { return shotCount; }
    }
    public void SaveAIMParam()
    {
        PlayerPrefs.SetFloat("G20_AIMMax", param.MaxValue);
        PlayerPrefs.SetFloat("G20_AIMDefault", param.DefaultValue);
        PlayerPrefs.SetFloat("G20_AIMOneChange", param.OneChangeValue);
        PlayerPrefs.Save();
        Debug.Log("AIMパラメーターをセーブしました。");
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
 
       
        Vector2? shotPoint = null;

        shotPoint = G20_InputPointGetter.GetInstance().GetInputPoint();
        
        if (shotPoint != null) shotCount++;
        if (isCheating && shotPoint == null)
        {
            shotPoint = G20_CheatShoot.GetInstance().GetEnemyHeadPoint();
        }
        if (CanShoot && shotPoint != null)
        {

            Vector3 hitPoint = Vector3.zero;
            //AIM補正
            if (aimAssistValue > 0)
            {
                shotPoint = G20_AIMAssistant.AssistAIM((Vector2)shotPoint, aimAssistValue);
            }
            var hitObj = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref hitPoint,Camera.main, fieldMask);
            if (hitObj)
            {
                hitObj.ExcuteActions(hitPoint);
                aimAssistValue -= param.OneChangeValue;
            }
            else
            {
                aimAssistValue += param.OneChangeValue;
            }
            aimAssistValue = Mathf.Clamp(aimAssistValue, 0, param.MaxValue);
            //effect出す用のパネルのRay判定
            Vector3 panelhitPoint = Vector3.zero;
            var hitPanel = G20_RayShooter.GetHitObject((Vector2)shotPoint, ref panelhitPoint,effectCamera,panelMask);
            if (hitPanel)
            {
                hitPanel.ExcuteActions(panelhitPoint);
            }
        }
    }
}
