using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public enum G20_EffectType
{
    HIT_APPLE_HEAD,
    HIT_APPLE_BODY,
    HIT_PANEL,
    SUMMON_APPLE,
    HIT_GOLDEN_HEAD,
    HIT_BOMB,
    HIT_PANEL2,
    PLUS_ONE_SCORE,
    HIT_PROTECT,
    SUMMON_APPLE_VERT,
}
public static class G20_EffectExt
{
    public static string GetTypeName(this G20_EffectType _type)
    {
        string retStr = "";
        switch (_type)
        {
            case G20_EffectType.HIT_APPLE_HEAD:
                retStr = "HitAppleHead2";
                break;
            case G20_EffectType.HIT_APPLE_BODY:
                retStr = "HitAppleBody";
                break;
            case G20_EffectType.HIT_PANEL:
                retStr = "HitPanel";
                break;
            case G20_EffectType.SUMMON_APPLE:
                retStr = "SummonApple";
                break;
            case G20_EffectType.HIT_GOLDEN_HEAD:
                retStr = "HitGoldenHead";
                break;
            case G20_EffectType.HIT_BOMB:
                retStr = "HitBomb";
                break;
            case G20_EffectType.HIT_PANEL2:
                retStr = "HitPanel2";
                break;
            case G20_EffectType.PLUS_ONE_SCORE:
                retStr = "PlusOneScore";
                break;
            case G20_EffectType.HIT_PROTECT:
                retStr = "HitProtect";
                break;
            case G20_EffectType.SUMMON_APPLE_VERT:
                retStr = "SummonApple_Vertical";
                break;
        }
        return retStr;
    }
}
public class G20_EffectManager : G20_Singleton<G20_EffectManager>
{
    Dictionary<int, GameObject> effectPrefabs=new Dictionary<int, GameObject>();
    protected override void Awake()
    {
        base.Awake();
        foreach (G20_EffectType i in Enum.GetValues(typeof(G20_EffectType)))
        {
            string resourcesName = "G20/Effect/" + i.GetTypeName();
            //Debug.Log(resourcesName);
            effectPrefabs.Add((int)i, (GameObject)Resources.Load(resourcesName, typeof(GameObject)));
        }
    }

    public GameObject Create(G20_EffectType effectType, Vector3 position)
    {
        var obj = Instantiate(effectPrefabs[(int)effectType], transform);
        obj.transform.position = position;
        return obj;
    }
}
