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
    [SerializeField] UnityEngine.UI.Image chainPanel;
    [SerializeField] ParticleSystem chainParticle;
	[SerializeField] int bonusValueEveryFive;
    public int ChainCount { get; private set; }
    public int MaxChainCount { get; private set; }
	[SerializeField] Gradient transitionColor;
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
		if ( ChainCount >= 2 )
		{
			chainText.text = ChainCount + "";
			chainAnim.Play( "G20_ChainUp", 0, 0.0f );

			Color textColor = transitionColor.Evaluate( (float)ChainCount / 7f );
			textColor.a = chainText.color.a;
			chainText.color = textColor;
			chainPanel.color = textColor;
			//ParticleSystem.MainModule par = chainParticle.main;
			//byte r = (byte)( textColor.r * 255 );
			//byte g = (byte)( textColor.g * 255 );
			//byte b = (byte)( textColor.b * 255 );
			//Debug.Log( r + " " + g + " " + b );
			//par.startColor = new ParticleSystem.MinMaxGradient( new Color( r, g, b, 255f ) );
			//chainParticle.Emit(10);
			//Debug.Log( "textColor : " + textColor );
			//Debug.Log( "chainColor : " + chainText.color );
		}
        ChainDurationTimer = ChainDurationSeconds;
    }
    void PlayChainSE(int chainCount)
    {
        switch (chainCount)
		{
			case 1:
			case 2:
			case 4:
			case 6:
				break;
			case 3:
                G20_SEManager.GetInstance().Play(G20_SEType.CHAIN1, Vector3.zero, false);
                break;
            case 5:
                G20_SEManager.GetInstance().Play(G20_SEType.CHAIN2, Vector3.zero, false);
                break;
            case 7:
                G20_SEManager.GetInstance().Play(G20_SEType.CHAIN3, Vector3.zero, false);
                break;
			default:
				G20_SEManager.GetInstance().Play( G20_SEType.CHAIN3, Vector3.zero, false );
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
