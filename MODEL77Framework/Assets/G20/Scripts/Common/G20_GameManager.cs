using System.Collections;
using UnityEngine;

// フレームワークとのやりとりと、
// タイトル→インゲーム→クリアまでの流れをさせる

public class G20_GameManager : G20_Singleton<G20_GameManager> {
    // フレームワークの関数を使うために必要
    private GameController _gameController;
    private CoordinateManager _coordinateManager;

    // GameController.cs から値をもらって保存しておく変数
    private int playerNum;
    private string[] playerId;

    private bool _isGameEnd;
    public event System.Action<G20_GameState> ChangedStateAction;
    G20_GameState gState = G20_GameState.TITLE;
    public G20_GameState gameState {
        get { return gState; }
        set {
            gState = value;
            if(ChangedStateAction!=null)ChangedStateAction(gState);
        }
    }

    [SerializeField]
    G20_Player playerObj;

    [SerializeField]
    GameObject titleApple;

    [SerializeField]
    GameObject uiTextSurvive;

    [SerializeField]
    GameObject titleCanvas;

    [SerializeField]
    GameObject ingameCanvas;

    void Start()
    {
        _gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        _coordinateManager = GameObject.Find("GameManager").GetComponent<CoordinateManager>();

        titleCanvas.SetActive(true);
        titleApple.SetActive(true);
        G20_BGMManager.GetInstance().Play(G20_BGMType.TITLE);
        ingameCanvas.SetActive(false);

        //Playerが死んだときにGameOverに移行させるためにアクションを追加
        playerObj.deathActions += (_) => GameFailed();
    }

    public void StartIngameCoroutine()
    {
        StartCoroutine(ToIngameCoroutine());
    }

    IEnumerator ToIngameCoroutine()
    {
        titleCanvas.SetActive(false);
        titleApple.SetActive(false);
        G20_BGMManager.GetInstance().FadeOut();
        yield return new WaitForSeconds(1);
        playerObj.GetComponent<Animator>().SetBool("zoomout", true);

        ingameCanvas.SetActive(true);

        // ボイス再生
        float voiceWait = 3.0f;

        G20_SEManager.GetInstance().Play(G20_SEType.TEST_VOICE, Vector3.zero, false);
        for (float t = 0; t < voiceWait; t+= Time.deltaTime )
        {
            // 待つ
            // デバッグボタン押されたら省略
            if ( G20_HitDebug.IsPressedButton )
            {
                break;
            }
            yield return null;
        }

        // デバッグボタンの押される猶予のため
        yield return null;

        uiTextSurvive.SetActive(true);
        // BGM
        G20_BGMManager.GetInstance().Play(G20_BGMType.INGAME_0);
        // インゲームスタート
        G20_StageManager.GetInstance().IngameStart();
        gameState = G20_GameState.INGAME;

        for (float t = 0; t < 3.0f; t += Time.deltaTime )
        {
            // デバッグボタン押されたら省略
            if ( G20_HitDebug.IsPressedButton )
            {
                break;
            }
            yield return null;
        }
        uiTextSurvive.SetActive(false);

        yield return null;
    }


    void Update()
    {

        if ( Input.GetKey(KeyCode.Escape) )
        {
            Application.Quit();
        }

        //// 画面に弾がヒットした場合 isUpdate() はtrueを返す。
        //if ( _coordinateManager.isUpdate() && !_isGameEnd )
        //{
        //    // CoordinateManagerから情報を取得。
        //    Hashtable res = _coordinateManager.Get();
        //    Vector3 pos = new Vector3((float)res["x"], (float)res["y"], 0f);

        //    // 受け取った座標をRayに変換
        //    Ray ray = Camera.main.ScreenPointToRay(pos);

        //    // Raycast処理
        //    RaycastHit hit = new RaycastHit();
        //    if ( Physics.Raycast(ray, out hit) )
        //    {
        //        GameObject hitObj = hit.collider.gameObject;

        //        PlayEffect(pos);

        //        if ( hitObj.name == "Target" )
        //            _targetCount--;

        //        _uiView.SetTargetCount(_targetCount);

        //        if ( _targetCount <= 0 )
        //        {
        //            _isGameEnd = true;
        //            _uiView.DisplayGameEnd();

        //            // scoreをGameControllerに送信。
        //            SendScore();

        //            // ゲームの終わりを通知し、メニュー画面へ
        //            _gameController.GameEnd();
        //        }
        //    }
        //}
    }

    public void SendScore()
    {
        var score = new int[1];
        score[0] = 10;
        var idm = playerId;
        var idate = new string[1];
        idate[0] = _gameController.Now();

        // 順番は配列の0番目から順にプレイヤー1, プレイヤー2・・・となる。
        // id: プレイヤーの識別番号
        // score: プレイヤーのスコア
        // idate: スコア送信時刻
        _gameController.ScoreUpdate(idm, score, idate);
    }
    public void GameFailed()
    {
        gameState = G20_GameState.GAMEOVER;
        Time.timeScale = 0;
        G20_BGMManager.GetInstance().Play(G20_BGMType.GAMEOVER);
        //ゲームオーバー演出の開始
        G20_FailedPerformer.GetInstance().Excute(GameEnd);
    }
    public void GameClear()
    {
        gameState = G20_GameState.CLEAR;

        // 敵（残ってれば）全滅
        G20_EnemyCabinet.GetInstance().KillAllEnemys();
        
        G20_BGMManager.GetInstance().Play(G20_BGMType.CLEAR);

        // クリア演出の開始
        G20_ClearPerformer.GetInstance().Excute(GameEnd);
    }

    public void GameEnd()
    {
        Debug.Log("ゲームエンド。フレームワークに処理を返します。");
        _gameController.GameEnd();
        //GameObject.Find("GameManager").GetComponent<GameController>().GameEnd();
    }
}
