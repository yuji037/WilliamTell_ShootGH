using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum G20_ScoreAppleType {
    APPLE,
    GOLDEN,
}

//public static class G20_ScoreAppleExt {
//    public static string GetTypeName(this G20_ScoreAppleType _type)
//    {
//        string retStr = "";
//        switch ( _type )
//        {
//            case G20_ScoreAppleType.APPLE:
//                retStr = "ScoreApple";
//                break;
//            case G20_ScoreAppleType.GOLDEN:
//                retStr = "ScoreGoldenApple";
//                break;
//        }

//        return retStr;
//    }
//}


public class G20_ScoreApplePopper : G20_Singleton<G20_ScoreApplePopper> {

    [SerializeField] GameObject[] scoreApplePrefabs;

    G20_HitScoreApple[] holdingApples;
    G20_ScoreApplePopPosition[] popPositions;
    bool isFullApples = false;

    //protected override void Awake()
    //{
        
    //}

    // Use this for initialization
    void Start () {
        popPositions = GetComponentsInChildren<G20_ScoreApplePopPosition>();
        holdingApples = new G20_HitScoreApple[popPositions.Length];
	}

    public void DeleteAllScoreApples()
    {
        for(int i = 0; i < holdingApples.Length; ++i )
        {
            if ( holdingApples[i] )
            {
                Destroy(holdingApples[i].gameObject);
            }
        }
    }

    public void PopSameAppleWithEnemy(G20_EnemyType enemyType)
    {
        int positionNumber = 0;
        G20_ScoreAppleType appleType = G20_ScoreAppleType.APPLE;

        if (    enemyType == G20_EnemyType.GOLDEN
            ||  enemyType == G20_EnemyType.GOLDEN_STRAIGHT )
            appleType = G20_ScoreAppleType.GOLDEN;


        if ( SearchEmptyPosition(out positionNumber) )
        {
            var obj = Instantiate(scoreApplePrefabs[(int)appleType], popPositions[positionNumber].transform);
            holdingApples[positionNumber] = obj.GetComponent<G20_HitScoreApple>();
        }

        // 全て埋まったかチェック
        isFullApples = true;
        foreach ( var a in holdingApples )
        {
            if ( !a )
            {
                isFullApples = false;
                break;
            }
        }
    }

    public void UnregisterApple(G20_HitScoreApple apple)
    {
        for(int i = 0; i < holdingApples.Length; ++i )
        {
            if(holdingApples[i] == apple )
            {
                holdingApples[i] = null;
                isFullApples = false;
            }
        }
    }

    bool SearchEmptyPosition(out int positionNumber)
    {
        positionNumber = 0;

        if ( isFullApples ) return false;
        
        bool isAlreadyPoppedNumber = false;

        int tryCount = 0;

        do
        {
            positionNumber = Random.Range(0, popPositions.Length);
            tryCount++;

            isAlreadyPoppedNumber = (holdingApples[positionNumber] != null);
        }
        while ( isAlreadyPoppedNumber && tryCount < 30);

        return true;
    }
}
