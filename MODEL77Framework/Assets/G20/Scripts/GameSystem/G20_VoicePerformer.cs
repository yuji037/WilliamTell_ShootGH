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

    Text serifuText;

	// Use this for initialization
	void Start () {
        serifuText = uiSerifuAnim.GetComponentInChildren<Text>();

        for ( int i = 0; i < serifuList.Length; i++ )
        {
        }
	}

    // ボイス番号0～最大まで
    public void Play(int voiceNumber)
    {
        StartCoroutine(PlayCoroutine(voiceNumber));
    }

    public void Play(G20_VoiceType voiceType)
    {
        StartCoroutine(PlayCoroutine((int)voiceType));
    }

    IEnumerator PlayCoroutine(int voiceNumber)
    {
        // 改行処理
        char[] c = { 'n' };

        var _strs = serifuList[voiceNumber].Split(c);

        serifuText.text = "";
        foreach ( var s in _strs )
        {
            serifuText.text += s;
            serifuText.text += "\n";
        }

        G20_SEType seType = G20_SEType.VOICE0 + voiceNumber;

        var sePlayer = G20_SEManager.GetInstance().Play(seType, Vector3.zero, false);

        uiSerifuAnim.CrossFade("Serifu_FadeIn", 0f);

        yield return new WaitForSeconds(2);
        while ( sePlayer.isPlaying )
        {
            yield return null;
        }

        uiSerifuAnim.CrossFade("Serifu_FadeOut", 0f);
    }

}
