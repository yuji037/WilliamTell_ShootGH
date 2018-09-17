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
    public System.Action OnGameEndAction;
    [SerializeField]
    G20_Player playerObj;

    [SerializeField]
    GameObject[] titleApples;

    [SerializeField]
    GameObject uiTextSurvive;

    [SerializeField]
    GameObject titleCanvas;

    [SerializeField]
    GameObject paramCanvas;

    [SerializeField]
    Animator gameRootAnim;

    [SerializeField]
    Color startAmbient;
    [SerializeField]
    Color ingameAmbient;
    [SerializeField]
    Color startFogColor;
    [SerializeField]
    float startFogDensity;
    [SerializeField]
    Color ingameFogColor;
    [SerializeField]
    float endFogIntensity;
    [SerializeField]
    Animator gesslerAnim;
    [SerializeField]
    bool isReloadMode;
    public bool isSkipPerformance = false;

    G20_GesslerShootPerformer gesslerShootPerformer;

    void Start()
    {
        _gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        _coordinateManager = GameObject.Find("GameManager").GetComponent<CoordinateManager>();
        gesslerShootPerformer = G20_ComponentUtility.FindComponentOnScene<G20_GesslerShootPerformer>();

        titleCanvas.SetActive(true);
        foreach(var apl in titleApples) apl.SetActive(true);
        G20_BGMManager.GetInstance().Play(G20_BGMType.TITLE);
        paramCanvas.SetActive(false);

        //Playerが死んだときにGameOverに移行させるためにアクションを追加
        playerObj.deathActions += (_) => GameFailed();

        RenderSettings.ambientLight = startAmbient;
        RenderSettings.fogColor = startFogColor;
        RenderSettings.fogDensity = startFogDensity;
    }

    public void StartIngameCoroutine()
    {
        StartCoroutine(ToIngameCoroutine());
    }

    IEnumerator ToIngameCoroutine()
    {
        // 演出中は弾の判定なし
        G20_BulletShooter.GetInstance().CanShoot = false;

        titleCanvas.SetActive(false);
        foreach ( var apl in titleApples ) apl.SetActive(false);
        G20_BGMManager.GetInstance().FadeOut();

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 1f);
        //playerObj.GetComponent<Animator>().SetBool("zoomout", true);

        // 環境光は別のコルーチンで遷移
        StartCoroutine(LightSettingCoroutine());


        // プレイヤー後ずさり等のアニメーション開始
        gameRootAnim.CrossFade("ToIngame", 0f);

        yield return new WaitForSeconds(7.2f);
        paramCanvas.SetActive(true);

        gameRootAnim.enabled = false;
        // ゲスラーふわふわアニメーション開始
        gesslerAnim.enabled = true;
        // 最初のリンゴ召喚
        G20_StageManager.GetInstance().IngameStart();
        G20_StageManager.GetInstance().nowStageBehaviour.SetEnableUpdateCall(1);

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 1.5f);

        // セリフ再生と字幕表示
        //G20_VoicePerformer.GetInstance().Play(0);

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 0.5f);

        //yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 1.0f);
        // 戦闘開始
        G20_BulletShooter.GetInstance().CanShoot = true;
        gameState = G20_GameState.INGAME;
        G20_EnemyCabinet.GetInstance().AllEnemyAIStart();
        // 「SURVIVE!」表示
        uiTextSurvive.SetActive(true);
        // BGM
        G20_BGMManager.GetInstance().Play(G20_BGMType.INGAME_0);

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 5f);

        uiTextSurvive.SetActive(false);

        yield return null;
    }

    IEnumerator LightSettingCoroutine()
    {
        //float changeLightTime = 1.0f;
        for ( float t = 0; t < 1f; t += Time.deltaTime )
        {
            RenderSettings.ambientLight = startAmbient * ( 1f - t ) + ingameAmbient * t;
            RenderSettings.fogColor = startFogColor * ( 1f - t ) + ingameFogColor * t;
            RenderSettings.fogDensity = startFogDensity * ( 1f - t ) + endFogIntensity * t;
            yield return null;
        }
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
        G20_VoicePerformer.GetInstance().PlayWithCaption(G20_VoiceType.GAME_OVER1);
        //ゲームオーバー演出の開始
        G20_FailedPerformer.GetInstance().Excute(GameEnd);
    }

    public void StartGesslerBattle()
    {
        //Debug.Log("GoldenApple : " + G20_Score.GetInstance().GoldPoint);
        gesslerAnim.enabled = false;
        // ゲスラー撃った後、クリア
        gesslerShootPerformer.ToGesslerBattle(GameClear);
    }

    public void GameClear()
    {
        gameState = G20_GameState.CLEAR;
        gameRootAnim.enabled = true;
        gameRootAnim.CrossFade("ToClear", 0.1f);
        StartCoroutine(RootAnimOff(2f));

        // 敵（残ってれば）全滅
        G20_EnemyCabinet.GetInstance().KillAllEnemys();
        G20_ScoreApplePopper.GetInstance().DeleteAllScoreApples();
        G20_BGMManager.GetInstance().FadeOut();

        // クリア演出の開始
        G20_ClearPerformer.GetInstance().Excute(GameEnd);
    }

    IEnumerator RootAnimOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameRootAnim.enabled = false;
    }
    void OnDestroy()
    {
        ChangedStateAction = null;
        OnGameEndAction = null;
        base.OnDestroy();
    }
    public void GameEnd()
    {
        Time.timeScale = 1f;
        if (OnGameEndAction != null) OnGameEndAction();
        //Debug.Log("ゲームエンド。フレームワークに処理を返します。");
        _gameController.GameEnd();
        if(isReloadMode) G20_ReloadScene.GetInstance().ReloadScene();
        //GameObject.Find("GameManager").GetComponent<GameController>().GameEnd();
    }
}
