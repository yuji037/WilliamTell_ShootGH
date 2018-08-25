﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class G20_StageBehaviour : MonoBehaviour {

    public enum NextSequenceConditionType {
        ENEMY_COUNT,
        INGAME_TIME,
        WAIT_TIME,
        NO_CONDITION,
    }

    public enum SequencePopType {
        CUE,
        PATERN,
    }

    public enum PopPaternType {
        PATERN_A,
        PATERN_B,
        PATERN_C,
    }

    [System.Serializable]
    public class PopSequence {
        public SequencePopType sequencePopType;
        public List<PopEnemyCue> addEnemyCueList = new List<PopEnemyCue>();
        public PopPaternType popPaternType;
        public float popIntervalTime;
        public int popEnemyCountByOneChance;
        public int limitEnemyCount;
        public NextSequenceConditionType conditionToNextSequence;
        public float conditionValue;
    }

    [System.Serializable]
    public class PopEnemyCue {
        public G20_EnemyType enemyType;
        public int positionNumber;
    }

    [System.Serializable]
    public class PopEnemyPatern {
        public PopEnemyCue[] cueList;
    }


    [SerializeField] PopSequence[] popSequenceList;

    [SerializeField] PopEnemyPatern paternA;
    [SerializeField] PopEnemyPatern paternB;
    [SerializeField] PopEnemyPatern paternC;

    List<PopEnemyCue> popCueList = new List<PopEnemyCue>();
    int limitEnemyCount = 0;

    G20_EnemyPopper enemyPopper;
    G20_EnemyCabinet enemyCabinet;
    G20_EnemyPopPosition[] popPositions;

    [SerializeField]
    private float timer = 0.0f;
    float thisSequenceStartTime = 0.0f;
    int sequenceCounter = 0;
    int paternCueCounter = 0;

    float popTimer = 0;

    G20_GameManager gameManager;


    // Use this for initialization
    void Start()
    {

        enemyPopper = G20_ComponentUtility.FindComponentOnScene<G20_EnemyPopper>();
        enemyCabinet = G20_EnemyCabinet.GetInstance();

        // ポップ位置情報の確保
        popPositions = enemyPopper.transform.GetComponentsInChildren<G20_EnemyPopPosition>();

        gameManager = G20_GameManager.GetInstance();

        SequenceEnter();
    }

    // Update is called once per frame
    void Update()
    {

        if ( gameManager.gameState == G20_GameState.GAMEOVER )
        {
            return;
        }

        if ( sequenceCounter >= popSequenceList.Length )
        {
            // これ以上敵出現しない
            return;
        }

        timer += Time.unscaledDeltaTime;
        popTimer += Time.unscaledDeltaTime;

        SequenceUpdate();

        // 状態遷移判定
        if ( IsNextStateCondition() )
        {
            sequenceCounter++;
            if ( sequenceCounter >= popSequenceList.Length )
            {
                // これ以上敵出現しない
                // ここにクリア判定がくるはず
                G20_GameManager.GetInstance().GameClear();
                Debug.Log("クリア");
                return;
            }

            SequenceEnter();
        }
    }

    void SequenceUpdate()
    {
        // Enemyをポップするかの判定
        var nowStatus = popSequenceList[sequenceCounter];

        if ( ( nowStatus.sequencePopType == SequencePopType.PATERN
            || ( nowStatus.sequencePopType == SequencePopType.CUE && popCueList.Count > 0 ))
            && popTimer >= nowStatus.popIntervalTime
            && enemyCabinet.enemyCount < nowStatus.limitEnemyCount )
        {
            popTimer = 0.001f;

            int popCount = nowStatus.popEnemyCountByOneChance;

            List<int> samePositionNumberList = new List<int>();

            while ( popCount > 0
                && enemyCabinet.enemyCount < nowStatus.limitEnemyCount )
            {
                if( nowStatus.sequencePopType == SequencePopType.CUE && popCueList.Count <= 0 )
                {
                    // キューが出尽くしている
                    break;
                }

                PopEnemyCue cue = new PopEnemyCue();

                switch ( nowStatus.sequencePopType )
                {
                    case SequencePopType.CUE:
                        // キューの先頭から抜き出す
                        cue = popCueList[0];
                        popCueList.RemoveAt(0);
                        break;

                    case SequencePopType.PATERN:
                        switch ( nowStatus.popPaternType )
                        {
                            case PopPaternType.PATERN_A:
                                cue = paternA.cueList[paternCueCounter];
                                paternCueCounter++; if ( paternCueCounter >= paternA.cueList.Length ) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_B:
                                cue = paternB.cueList[paternCueCounter];
                                paternCueCounter++; if ( paternCueCounter >= paternB.cueList.Length ) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_C:
                                cue = paternC.cueList[paternCueCounter];
                                paternCueCounter++; if ( paternCueCounter >= paternC.cueList.Length ) paternCueCounter = 0;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                popCount--;

                int positionNumber = cue.positionNumber - 1;

                // 位置ランダム選択の失敗回数。
                // これがないとバグで止まるとは今のところ確認されてない（ほぼ要らない）
                int tryCount = 0;

                if ( cue.positionNumber == 0 )
                {
                    bool isSameNumber = false;
                    do
                    {
                        positionNumber = Random.Range(0, 15);
                        //Debug.Log("位置選出");

                        isSameNumber = false;
                        foreach (var num in samePositionNumberList )
                        {
                            if ( positionNumber == num ) isSameNumber = true;
                        }
                        tryCount++;
                    } while ( isSameNumber && samePositionNumberList.Count < 15 && tryCount < 30);
                }
                if ( positionNumber < 0 || positionNumber > 14 )
                {
                    Debug.LogError("ポップ位置の選択が不正");
                }
                Vector3 _popPos = popPositions[positionNumber].transform.position;
                samePositionNumberList.Add(positionNumber);
                // 敵出現
                enemyPopper.EnemyPop(cue.enemyType, _popPos);
            }
        }
    }

    void SequenceEnter()
    {
        Debug.Log("EnemyPop Sequence " + sequenceCounter);

        paternCueCounter = 0;
        thisSequenceStartTime = timer;

        // すぐにポップできるようにする
        popTimer = 999.0f;

        var nowSequence = popSequenceList[sequenceCounter];

        // このシーケンスタイプがPATERNなら今持ってるキューを全部捨てる
        if ( nowSequence.sequencePopType == SequencePopType.PATERN )
        {
            popCueList.Clear();
        }

        // 順番待ちリストに新規追加
        foreach ( var enemy in nowSequence.addEnemyCueList )
        {
            popCueList.Add(enemy);
        }
    }


    bool IsNextStateCondition()
    {
        var nowStatus = popSequenceList[sequenceCounter];

        switch ( nowStatus.conditionToNextSequence )
        {

            case NextSequenceConditionType.ENEMY_COUNT:

                return enemyCabinet.enemyCount <= nowStatus.conditionValue;

            case NextSequenceConditionType.INGAME_TIME:

                return timer >= nowStatus.conditionValue;

            case NextSequenceConditionType.WAIT_TIME:

                return timer >= thisSequenceStartTime + nowStatus.conditionValue;

            case NextSequenceConditionType.NO_CONDITION:

                return true;

            default:
                Debug.LogWarning("エラー：StageBehaviourのNextStateConditionの設定がおかしい");
                return true;
        }
    }
}
