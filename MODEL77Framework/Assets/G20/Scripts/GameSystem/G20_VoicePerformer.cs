using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        G20_SEType seType = G20_SEType.VOICE1 + voiceNumber;

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
