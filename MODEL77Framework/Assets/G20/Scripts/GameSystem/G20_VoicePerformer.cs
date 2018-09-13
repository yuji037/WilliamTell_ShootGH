using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum G20_VoiceType {
    GAME_START1,
    GAME_START2,
    GAME_START3,
    INGAME1,
    INGAME2,
    INGAME3,
    INGAME4,
    INGAME5,
    INGAME6,
    INGAME7,
    GAME_CLEAR1,
    GAME_CLEAR2,
    GAME_CLEAR3,
    GAME_CLEAR4,
    GAME_CLEAR5,
    GAME_OVER1,
    GAME_OVER2,
    GAME_OVER3,
    GAME_OVER4,
    GAME_OVER5,
}


public class G20_VoicePerformer : G20_Singleton<G20_VoicePerformer> {

    [SerializeField]
    string[] serifuList;

    [SerializeField]
    Animator uiSerifuAnim;

    //Text serifuText;

    [SerializeField]
    G20_CaptionPerformer captionPerformer;

	// Use this for initialization
	void Start () {
        //serifuText = uiSerifuAnim.GetComponentInChildren<Text>();
	}

    // 字幕表示含む再生
    public void PlayWithCaption(G20_VoiceType voiceType)
    {
        StartCoroutine(PlayCoroutine((int)voiceType));
    }
    
    IEnumerator PlayCoroutine(int voiceNumber)
    {
        //// 改行処理
        //char[] c = { 'n' };

        //var _strs = serifuList[voiceNumber].Split(c);

        //serifuText.text = "";
        //for(int i = 0; i < _strs.Length; ++i )
        //{
        //    serifuText.text += _strs[i];
        //    if ( i < _strs.Length - 1 ) serifuText.text += "\n";
        //}
        
        G20_SEType seType = G20_SEType.VOICE0 + voiceNumber;

        var sePlayer = G20_SEManager.GetInstance().Play(seType, Vector3.zero, false);

        //uiSerifuAnim.CrossFade("Serifu_FadeIn", 0f);
        captionPerformer.StartPerformance(serifuList[voiceNumber]);
        float seLength = G20_SEManager.GetInstance().GetClipLength(seType);
        float minLength = 1.0f;
        float displayCaptionLength = Mathf.Max(minLength, seLength - 1.0f);
        yield return new WaitForSeconds(displayCaptionLength);
        captionPerformer.StopPerformance();
        //uiSerifuAnim.CrossFade("Serifu_FadeOut", 0f);
    }

    // 字幕表示なし
    // 再生中はBGM音量下げる
    public void PlayWithNoCaption(G20_VoiceType voiceNumber)
    {
        // 字幕表示しないボイス再生
        G20_SEType seType = G20_SEType.VOICE0 + (int)voiceNumber;
        var sePlayer = G20_SEManager.GetInstance().Play(seType, Vector3.zero, false);
        float clipLength = G20_SEManager.GetInstance().GetClipLength(seType);
        G20_BGMManager.GetInstance().VolumeDown(clipLength);
    }

    // 字幕表示なし
    // BGMも音量そのまま
    // 扱いは効果音と同じ
    public void PlayWithNoControll(G20_VoiceType voiceType)
    {

    }
}
