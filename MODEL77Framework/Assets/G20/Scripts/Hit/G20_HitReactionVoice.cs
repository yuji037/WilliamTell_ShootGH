using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitReactionVoice : G20_HitAction {

    [SerializeField]
    float reactionInterval = 2.0f;

    float reactionIntervalTimer = 99f;

    int prePlayNum = 0;

    [SerializeField] List<G20_VoiceType> voices;
    public void ChangeVoices(List<G20_VoiceType> _voices)
    {
        voices = _voices;
        prePlayNum = 0;
    }
    public bool isRandomPlay=true;
	// Update is called once per frame
	void Update () {
        reactionIntervalTimer += Time.deltaTime;
	}

    public override void Execute(Vector3 hit_point)
    {
        if(reactionIntervalTimer > reactionInterval )
        {
            // 他ボイス再生中は再生しない
            if ( G20_VoicePerformer.GetInstance().IsPlaying ) return;
            reactionIntervalTimer = 0f;

            int playNum = 0;
            if (isRandomPlay)
            {
                do { playNum = Random.Range(0, voices.Count); }
                while (prePlayNum == playNum);
            }
            else
            {
                //listの並び順に再生
                playNum=prePlayNum+1;
                if (playNum >= voices.Count)
                {
                    playNum = 0;
                }
            }
            
            G20_VoicePerformer.GetInstance().PlayWithNoControll(voices[playNum]);
            prePlayNum = playNum;
        }
    }
}
