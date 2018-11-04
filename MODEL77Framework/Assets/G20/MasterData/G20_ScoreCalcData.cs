using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class G20_ScoreCalcData : ScriptableObject
{
    public int MaxChainMultiply;
    public int HitRateMultyply;

    [MenuItem("Assets/Create/G20_ScoreCalcData")]

    static void CreateScoreCalcData()
    {
        G20_ScoreCalcData instance = CreateInstance<G20_ScoreCalcData>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/G20/MasterData/Data/G20_ScoreCalcData.asset");
        AssetDatabase.CreateAsset(instance, path);
        AssetDatabase.Refresh();
    }

}
#endif
