using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// フレームワークとのやりとりと、
// タイトル→インゲーム→クリアまでの流れをさせる

public class G20_GameManager : G20_Singleton<G20_GameManager>
{
    // フレームワークの関数を使うために必要
    private GameController _gameController;
    private CoordinateManager _coordinateManager;

    private bool _isGameEnd;
    public event System.Action<G20_GameState> ChangedStateAction;

    //ゲーム難易度
    public int gameDifficulty;

    G20_GameState gState = G20_GameState.TITLE;
    public G20_GameState gameState
    {
        get { return gState; }
        set
        {
            gState = value;
            if (ChangedStateAction != null) ChangedStateAction(gState);
        }
    }
    public System.Action OnGameEndAction;
    [SerializeField]
    G20_Player playerObj;


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
    [SerializeField]
    ParticleSystem summonParticle;
    public bool isSkipPerformance = false;

    G20_GesslerShootPerformer gesslerShootPerformer;

    void Start()
    {
        _gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        _coordinateManager = GameObject.Find("GameManager").GetComponent<CoordinateManager>();
        gesslerShootPerformer = G20_ComponentUtility.FindComponentOnScene<G20_GesslerShootPerformer>();

        titleCanvas.SetActive(true);
        G20_BGMManager.GetInstance().Play(G20_BGMType.TITLE);
        paramCanvas.SetActive(false);

        //Playerが死んだときにGameOverに移行させるためにアクションを追加
        playerObj.deathActions += (x, y) => GameFailed();

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
        G20_BGMManager.GetInstance().FadeOut();
        var seForest = G20_SEManager.GetInstance().Play(G20_SEType.FOREST, Vector3.zero, false);

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 1f);
        //playerObj.GetComponent<Animator>().SetBool("zoomout", true);

        // 環境光は別のコルーチンで遷移
        StartCoroutine(LightSettingCoroutine());


        // プレイヤー後ずさり等のアニメーション開始
        gameRootAnim.CrossFade("ToIngame", 0f);

        yield return new WaitForSeconds(5.5f);
        summonParticle.Play();
        yield return new WaitForSeconds(0.7f);
        paramCanvas.SetActive(true);
        summonParticle.Stop();

        gesslerAnim.enabled = true;

        gesslerShootPerformer.gesslerAnim.PlayAnim(G20_GesslerAnimType.Attack);
        yield return new WaitForSeconds(0.9f);
        yield return new WaitForSeconds(0.6f);

        gesslerShootPerformer.gesslerAnim.PlayAnim(G20_GesslerAnimType.Taiki);

        // ゲスラーふわふわアニメーション開始

        // 最初のリンゴ召喚
        G20_StageManager.GetInstance().IngameStart();
        G20_StageManager.GetInstance().nowStageBehaviour.SetEnableUpdateCall(1);


        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 1.5f);

        // セリフ再生と字幕表示
        //G20_VoicePerformer.GetInstance().Play(0);

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 0.5f);
        gameRootAnim.enabled = false;
        gesslerShootPerformer.gesslerAnim.PlayAnim(G20_GesslerAnimType.Taiki);

        // 戦闘開始
        G20_BulletShooter.GetInstance().CanShoot = true;
        gameState = G20_GameState.INGAME;
        G20_EnemyCabinet.GetInstance().AllEnemyAIStart();
        // 「SURVIVE!」表示
        uiTextSurvive.SetActive(true);
        // BGM
        G20_BGMManager.GetInstance().Play(G20_BGMType.INGAME_0);
        G20_SEManager.GetInstance().Fadeout(seForest);

        yield return new WaitForSeconds(isSkipPerformance ? 0.001f : 5f);

        uiTextSurvive.SetActive(false);

        yield return null;
    }

    IEnumerator LightSettingCoroutine()
    {
        //float changeLightTime = 1.0f;
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            RenderSettings.ambientLight = startAmbient * (1f - t) + ingameAmbient * t;
            RenderSettings.fogColor = startFogColor * (1f - t) + ingameFogColor * t;
            RenderSettings.fogDensity = startFogDensity * (1f - t) + endFogIntensity * t;
            yield return null;
        }
    }


    public void SendScore()
    {
        GameController GC = GameObject.Find("GameManager").GetComponent<GameController>();
        string[] idm = new string[1];//プレイヤーID
        int[] score = new int[1];//スコア
        string[] idate = new string[1];//プレイ終了時間

        int cnt = 0;
        for (int i = 0; i <= GC.player_isentry.Length; i++)
        {
            if (GC.player_isentry[i] == true)//player_isentryがtrueの人が参加
            {
                idm[cnt] = GC.player_id[i];
                cnt++;
                break;
            }
        }
        //現在時刻を取得
        idate[0] = GC.Now();
        //スコア取得
        score[0] = G20_ScoreManager.GetInstance().GetSumScore();

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
    public bool IsGesslerBattle
    {
        get; private set;
    }
    public void StartGesslerBattle()
    {
        gesslerAnim.enabled = false;
        IsGesslerBattle = true;
        gesslerShootPerformer.ToGesslerBattle(GameClear);
    }

    public void GameClear()
    {
        IsGesslerBattle = false;
        gameState = G20_GameState.CLEAR;
        //gameRootAnim.enabled = true;
        //gameRootAnim.CrossFade("ToClear", 0.1f);
        //StartCoroutine(RootAnimOff(2f));

        // 敵（残ってれば）全滅
        G20_EnemyCabinet.GetInstance().KillAllEnemys();
        G20_ScoreApplePopper.GetInstance().DeleteAllScoreApples();
        G20_BGMManager.GetInstance().FadeOut();
        G20_BulletShooter.GetInstance().coutingHitRate = false;
        // クリア演出の開始
        G20_ClearPerformer.GetInstance().Excute(GameEnd);
    }

    IEnumerator RootAnimOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameRootAnim.enabled = false;
    }

    new void OnDestroy()
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
        if (isReloadMode) G20_ReloadScene.GetInstance().ReloadScene();
        Resources.UnloadUnusedAssets();
    }
}
