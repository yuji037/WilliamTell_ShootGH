using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_StreakCounter : MonoBehaviour {
    //ストリークの持続時間
    [SerializeField] float streakDuration;
    [SerializeField] int streakValue;
    [SerializeField] Animator streakAnim;
    [SerializeField] UnityEngine.UI.Text streakText;
    [SerializeField] float showTime;
    float timer=0f;
    //ストリークを数え始めるカウント
    int startScore;
    // Use this for initialization
	void Start () {
        G20_Score.GetInstance().ScoreChangedAction += Count;	
	}
    void Count(int _count)
    {
        if (0>=timer)
        {
            startScore = _count;
            StartCoroutine(StreakCoroutine());
        }
        timer = streakDuration;
    }
	// Update is called once per frame
	IEnumerator StreakCoroutine()
    {
        yield return null;
        while (true)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME) yield break;
            timer -= Time.deltaTime;
            if (0 >= timer)
            {
                if (G20_Score.GetInstance().Score - startScore >= streakValue)
                {
                    
                    yield return StartCoroutine(ShowStreak());
                }
                yield break;
            }
            yield return null;
        }
	}
    IEnumerator ShowStreak()
    {
        streakText.text = (G20_Score.GetInstance().Score- startScore).ToString();
        streakAnim.CrossFade("Serifu_FadeIn", 0f);
        yield return new WaitForSeconds(showTime);
        streakAnim.CrossFade("Serifu_FadeOut", 0f);
    }
}
