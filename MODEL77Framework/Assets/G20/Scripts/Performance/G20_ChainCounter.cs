using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_ChainCounter : G20_Singleton<G20_ChainCounter>
{
    //ストリークの持続時間
    [SerializeField] float ChainDurationSeconds;
    float ChainDurationTimer;
    [SerializeField] Animator chainAnim;
    [SerializeField] UnityEngine.UI.Text chainText;
    [SerializeField] int bonusValueEveryFive;
    public int ChainCount { get; private set; }
    public int MaxChainCount { get; private set; }
    // Use this for initialization
    private void Awake()
    {
        G20_BulletShooter.GetInstance().ActionHitObject += UpdateCount;
    }
    private void Update()
    {
        ChainDurationTimer -= Time.deltaTime;
        if (ChainDurationTimer <= 0)
        {
            ResetChainCount();
        }
    }
    void ResetChainCount()
    {
        if (2 <= ChainCount)
        {
            chainAnim.CrossFade("G20_ChainFadeOut", 0f);
        }
        ChainCount = 0;
    }

    public int GetOneTimeBonusScore()
    {
        return (ChainCount / 5) * bonusValueEveryFive;
    }

    public void UpChainCount()
    {
        ChainCount++;
        PlayChainSE(ChainCount);
        if (ChainCount > MaxChainCount) MaxChainCount = ChainCount;
        //2chain目だった場合スコア表示アニメーション実行
        if (ChainCount == 2)
        {
            chainAnim.CrossFade("G20_ChainFadeIn", 0f);
        }
        if (ChainCount >= 2)
        {
            chainText.text = ChainCount + "";
            chainAnim.Play("G20_ChainUp", 0, 0.0f);
        }
        ChainDurationTimer = ChainDurationSeconds;
    }
    void PlayChainSE(int chainCount)
    {
        switch (chainCount)
        {
            case 3:
                G20_SEManager.GetInstance().Play(G20_SEType.CHAIN1, Vector3.zero, false);
                break;
            case 5:
                G20_SEManager.GetInstance().Play(G20_SEType.CHAIN2, Vector3.zero, false);
                break;
            case 7:
                G20_SEManager.GetInstance().Play(G20_SEType.CHAIN3, Vector3.zero, false);
                break;
        }
    }
    void UpdateCount(G20_HitObject hitObject)
    {
        if (!(hitObject && hitObject.IsHitRateUp))
        {
            ResetChainCount();
        }
    }
}
