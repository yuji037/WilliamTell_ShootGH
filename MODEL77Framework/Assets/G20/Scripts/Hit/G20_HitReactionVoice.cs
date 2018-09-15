using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitReactionVoice : G20_HitAction {

    [SerializeField]
    float reactionInterval = 2.0f;

    float reactionIntervalTimer = 99f;

    int prePlayNum = 0;

    [SerializeField]
    G20_VoiceType[] voices;
	
	// Update is called once per frame
	void Update () {
        reactionIntervalTimer += Time.deltaTime;
	}

    public override void Execute(Vector3 hit_point)
    {
        if(reactionIntervalTimer > reactionInterval )
        {
            reactionIntervalTimer = 0f;
            int randNum = 0;
            do{randNum = Random.Range(0, voices.Length);}
            while ( prePlayNum == randNum );

            G20_VoicePerformer.GetInstance().PlayWithNoControll(voices[randNum]);
            prePlayNum = randNum;
        }
    }
}
