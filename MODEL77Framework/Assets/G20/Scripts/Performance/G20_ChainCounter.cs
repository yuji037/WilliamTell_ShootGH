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
    int chainCount;
    // Use this for initialization
    private void Awake()
    {
        G20_BulletShooter.GetInstance().ActionHitObject += CountUpdate;
    }
    private void Update()
    {
        ChainDurationTimer -= Time.deltaTime;
        if (ChainDurationTimer<=0)
        {
            CutChain();
        }
    }
    void CutChain()
    {
        if (2 <= chainCount)
        {
            chainAnim.CrossFade("Serifu_FadeOut", 0f);
        }
        chainCount = 0;
        chainText.text = "";
    }
    public void UpChainCount()
    {
        chainCount++;
        //2chain目だった場合スコア表示アニメショーン実行
        if (chainCount == 2)
        {
            chainAnim.CrossFade("Serifu_FadeIn", 0f);
        }
        chainText.text = chainCount + "CHAIN";
        ChainDurationTimer = ChainDurationSeconds;
    }
    void CountUpdate(G20_HitObject hitObject)
    {
        if (!(hitObject &&hitObject.IsHitRateUp))
        {
            CutChain();
        }

    }
}
