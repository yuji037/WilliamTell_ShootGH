using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class G20_StageBehaviour : MonoBehaviour
{

    public enum NextSequenceConditionType
    {
        ENEMY_COUNT,
        INGAME_TIME,
        WAIT_TIME,
        NO_CONDITION,
    }

    public enum SequencePopType
    {
        CUE,
        PATERN,
    }

    public enum PopPaternType
    {
        PATERN_A,
        PATERN_B,
        PATERN_C,
        PATERN_D,
        PATERN_E,
        PATERN_F,
        PATERN_G,
        PATERN_H,
    }

    [SerializeField, Range(0, 90)]
    public float stageTotalTime = 90.0f;

    [System.Serializable]
    public class PopSequence
    {
        [SerializeField, Header("シーケンスタイプ")] public SequencePopType sequencePopType;
        [SerializeField, Header("シーケンスタイプがキューだった場合ここを編集")] public List<PopEnemyCue> addEnemyCueList = new List<PopEnemyCue>();
        [SerializeField, Header("シーケンスタイプがパターンだった場合ここを編集")] public PopPaternType popPaternType;
        [SerializeField, Header("エネミーが出るまでの間隔")] public float popIntervalTime;
        [SerializeField, Header("一回のポップ処理で出るエネミーの数")] public int popEnemyCountByOneChance;
        [SerializeField, Header("フィールド上に存在出来るエネミーの数")] public int limitEnemyCount;
        [Tooltip("EnemyCount:フィールドのエネミー数がConditionValue以下の場合次シーケンスに移行\n"+
        "IngameTime:\tタイマーがConditionValue秒数以下の場合移行\n"+
        "WaitTime:ConditionValue秒経ったら移行\n"+
        "NoCondition:条件無しで移行\n")]
        [SerializeField] public NextSequenceConditionType conditionToNextSequence;
        [SerializeField, Header("移る条件で使う値")] public float conditionValue;
        [SerializeField, Header("エネミーに加算するスピード")] public float speedBuffValue;
        [SerializeField, Header("演出が入る場合はここに、G20_IngamePerformerをアタッチしたprefabを挿入")] public GameObject InGamePerformer;
    }

    [System.Serializable]
    public class PopEnemyCue
    {
        public G20_EnemyType enemyType;
        [SerializeField, Range(-5, 22)]
        public int positionNumber;
    }

    [System.Serializable]
    public class PopEnemyPatern
    {
        public PopEnemyCue[] cueList;
    }

    [SerializeField, Header("シーケンス")] PopSequence[] popSequenceList;

    [SerializeField] PopEnemyPatern paternA;
    [SerializeField] PopEnemyPatern paternB;
    [SerializeField] PopEnemyPatern paternC;
    [SerializeField] PopEnemyPatern paternD;
    [SerializeField] PopEnemyPatern paternE;
    [SerializeField] PopEnemyPatern paternF;
    [SerializeField] PopEnemyPatern paternG;
    [SerializeField] PopEnemyPatern paternH;

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

    float popTimer = 0f;
    float scoreApplePopTimer = 0f;

    [SerializeField]
    float scoreApplePopInterval = 3.0f;

    // スコアアップルはステージ終了間際ではポップさせない
    float scoreAppleCanPopTimeMin = 10f;
    int scoreApplePaternCueCounter = 0;

    [SerializeField]
    G20_ScoreAppleType[] scoreApplePatern;

    G20_GameManager gameManager;

    // 空中から出現する


    // Use this for initialization
    void Start()
    {
        timer = stageTotalTime;

        enemyPopper = G20_ComponentUtility.FindComponentOnScene<G20_EnemyPopper>();
        enemyCabinet = G20_EnemyCabinet.GetInstance();

        // ポップ位置情報の確保
        popPositions = enemyPopper.transform.GetComponentsInChildren<G20_EnemyPopPosition>();
        Array.Sort(popPositions, (a, b) => a.number - b.number);
        gameManager = G20_GameManager.GetInstance();

        scoreAppleCanPopTimeMin = stageTotalTime * 0.1f;

        SequenceEnter();
    }

    int updateCall = 0;

    public void SetEnableUpdateCall(int callCount)
    {
        updateCall = callCount;
    }
    //デバッグ用
    public void EndStage()
    {
        sequenceCounter = popSequenceList.Length;
        G20_GameManager.GetInstance().StartGesslerBattle();
        //Debug.Log("ゲスラー戦");
    }
    // Update is called once per frame
    void Update()
    {

        if (updateCall <= 0 && gameManager.gameState != G20_GameState.INGAME)
        {
            return;
        }
        updateCall--;

        if (sequenceCounter >= popSequenceList.Length)
        {
            // これ以上敵出現しない
            return;
        }

        if (gameManager.gameState == G20_GameState.INGAME)
        {
            timer -= Time.deltaTime;
            popTimer += Time.deltaTime;
        }

        SequenceUpdate();
        ScoreAppleUpdate();

        // 状態遷移判定
        if (IsNextStateCondition())
        {
            sequenceCounter++;
            if (sequenceCounter < popSequenceList.Length)
            {
                //シーケンスに設定されたインゲームの演出開始
                if (popSequenceList[sequenceCounter].InGamePerformer)
                {
                    popSequenceList[sequenceCounter].InGamePerformer.GetComponent<G20_IngamePerformer>().StartPerformance();
                }
            }
            else
            {
                // これ以上敵出現しない
                // ゲスラー戦へ移行
                G20_GameManager.GetInstance().StartGesslerBattle();
                //Debug.Log("ゲスラー戦");
                return;
            }

            SequenceEnter();
        }
    }

    void ScoreAppleUpdate()
    {
        if ( scoreApplePatern.Length == 0 ) return;

        scoreApplePopTimer += Time.deltaTime;

        if(timer > scoreAppleCanPopTimeMin
            && scoreApplePopTimer > scoreApplePopInterval )
        {
            scoreApplePopTimer = 0f;
            G20_ScoreApplePopper.GetInstance().PopApple(scoreApplePatern[scoreApplePaternCueCounter]);
            scoreApplePaternCueCounter++;
            if ( scoreApplePaternCueCounter >= scoreApplePatern.Length ) scoreApplePaternCueCounter = 0;
        }
    }

    void SequenceUpdate()
    {
        // Enemyをポップするかの判定
        var nowStatus = popSequenceList[sequenceCounter];
        if ((nowStatus.sequencePopType == SequencePopType.PATERN
            || (nowStatus.sequencePopType == SequencePopType.CUE && popCueList.Count > 0))
            && popTimer >= nowStatus.popIntervalTime
            && enemyCabinet.enemyCount < nowStatus.limitEnemyCount)
        {
            popTimer = 0.001f;

            int popCount = nowStatus.popEnemyCountByOneChance;

            List<int> poppedNumberList = new List<int>();

            while (popCount > 0
                && enemyCabinet.enemyCount < nowStatus.limitEnemyCount)
            {
                if (nowStatus.sequencePopType == SequencePopType.CUE && popCueList.Count <= 0)
                {
                    // キューが出尽くしている
                    break;
                }

                PopEnemyCue cue = new PopEnemyCue();

                switch (nowStatus.sequencePopType)
                {
                    case SequencePopType.CUE:
                        // キューの先頭から抜き出す
                        cue = popCueList[0];
                        popCueList.RemoveAt(0);
                        break;

                    case SequencePopType.PATERN:
                        switch (nowStatus.popPaternType)
                        {
                            case PopPaternType.PATERN_A:
                                cue = paternA.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternA.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_B:
                                cue = paternB.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternB.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_C:
                                cue = paternC.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternC.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_D:
                                cue = paternD.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternD.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_E:
                                cue = paternE.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternE.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_F:
                                cue = paternF.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternF.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_G:
                                cue = paternG.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternG.cueList.Length) paternCueCounter = 0;
                                break;
                            case PopPaternType.PATERN_H:
                                cue = paternH.cueList[paternCueCounter];
                                paternCueCounter++; if (paternCueCounter >= paternH.cueList.Length) paternCueCounter = 0;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                popCount--;
                int positionNumber = 0;

                //0以下だったらランダム処理
                if (cue.positionNumber <= 0)
                {
                    int minNum = 0;
                    int maxNum = 0;
                    SetMinMAx(cue.positionNumber, out minNum, out maxNum);
                    positionNumber = GetRandomPopNumber(poppedNumberList, minNum, maxNum);
                }
                else
                {
                    positionNumber = cue.positionNumber - 1;
                }

                if (positionNumber < 0 || popPositions.Length < positionNumber)
                {
                    //Debug.LogError("ポップ位置の選択が不正");
                }
                Vector3 _popPos = popPositions[positionNumber].transform.position;
                poppedNumberList.Add(positionNumber);

                G20_EnemyPopType popType = G20_EnemyPopType.RISE_UP;

                if ( positionNumber <= 19 )
                    popType = G20_EnemyPopType.RISE_UP;
                else if ( 20 <= positionNumber && positionNumber <= 23 ) {
                    popType = G20_EnemyPopType.WALK_FORWARD; }

                // 敵出現
                var enemy = enemyPopper.EnemyPop(cue.enemyType, _popPos, popType);

                // 同時に試しにステージ脇リンゴ生成
                //G20_ScoreApplePopper.GetInstance().PopSameAppleWithEnemy(cue.enemyType);

                // スピードバフ
                G20_SpeedBuff speedBuff = null;
                if (nowStatus.speedBuffValue != 0) speedBuff = new G20_SpeedBuff(enemy, 100.0f, nowStatus.speedBuffValue);

                if (speedBuff != null) enemy.AddBuff(speedBuff);
            }
        }
    }
    void SetMinMAx(int position_number, out int _min, out int _max)
    {
        switch (position_number)
        {
            case -5:
                _min = 21;
                _max = 24;
                break;
            case -4:
                _min = 15;
                _max = 19;
                break;
            case -3:
                _min = 10;
                _max = 14;
                break;
            case -2:
                _min = 5;
                _max = 9;
                break;
            case -1:
                _min = 0;
                _max = 4;
                break;
            case 0:
            default:
                _min = 0;
                _max = 19;
                break;
        }
    }

    //pop出来るpostionが一つでもあればtrueを返す
    bool CanPop(List<int> popped_numberlist, int min_num, int max_num)
    {
        int maxCount = max_num - min_num;
        int count = 0;
        foreach (var i in popped_numberlist)
        {
            if (min_num <= i && i <= max_num)
            {
                count++;
                break;
            }
        }
        return maxCount > count;
    }
    //ポップできる有効なランダム値を取得できるまで再帰する。
    int GetRandomPopNumber(List<int> popped_numberlist, int min_num, int max_num)
    {
        if (popped_numberlist.Count > (max_num - min_num)) return 0;
        var randNum = UnityEngine.Random.Range(min_num, max_num + 1);
        foreach (var p in popped_numberlist)
        {
            if (p == randNum)
            {
                randNum = GetRandomPopNumber(popped_numberlist, min_num, max_num);
                break;
            }
        }
        return randNum;
    }
    void SequenceEnter()
    {
        //Debug.Log("EnemyPop Sequence " + sequenceCounter);

        paternCueCounter = 0;
        thisSequenceStartTime = timer;

        // すぐにポップできるようにする
        popTimer = 999.0f;

        var nowSequence = popSequenceList[sequenceCounter];

        // このシーケンスタイプがPATERNなら今持ってるキューを全部捨てる
        if (nowSequence.sequencePopType == SequencePopType.PATERN)
        {
            popCueList.Clear();
        }

        // 順番待ちリストに新規追加
        foreach (var enemy in nowSequence.addEnemyCueList)
        {
            popCueList.Add(enemy);
        }
    }


    bool IsNextStateCondition()
    {
        var nowSequence = popSequenceList[sequenceCounter];

        switch (nowSequence.conditionToNextSequence)
        {

            case NextSequenceConditionType.ENEMY_COUNT:

                return enemyCabinet.enemyCount <= nowSequence.conditionValue;

            case NextSequenceConditionType.INGAME_TIME:

                return timer <= nowSequence.conditionValue;

            case NextSequenceConditionType.WAIT_TIME:

                return timer <= thisSequenceStartTime - nowSequence.conditionValue;

            case NextSequenceConditionType.NO_CONDITION:

                return true;

            default:
                //Debug.LogWarning("エラー：StageBehaviourのNextStateConditionの設定がおかしい");
                return true;
        }
    }
}
