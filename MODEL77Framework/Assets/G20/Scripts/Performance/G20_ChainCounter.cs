using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_ChainCounter : MonoBehaviour
{
    //ストリークの持続時間
    [SerializeField] float ChainDurationSeconds;
    float ChainDurationTimer;
    [SerializeField] Animator chainAnim;
    [SerializeField] UnityEngine.UI.Text chainText;
    int chainCount;
    float timer = 0f;
    //ストリークを数え始めるカウント
    int startScore;
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
    void CountUpdate(G20_HitObject hitObject)
    {
        //hitObjectに当たり、タグがアシストだった場合chainCount加算
        if (hitObject != null && hitObject.HitTag == G20_HitTag.ASSIST)
        {
            chainCount++;
            //2chain目だった場合スコア表示アニメショーン実行
            if (chainCount == 2)
            {
                chainAnim.CrossFade("Serifu_FadeIn", 0f);
            }
            chainText.text = chainCount+"CHAIN";
            ChainDurationTimer = ChainDurationSeconds;
        }
        else
        {
            CutChain();
        }

    }
}
